using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode
{
	FREE,			// 3D
	TRANSITIONING,	// Transitioning from 3D to 2D or 2D to 3D
	CLASH  			// 2D
}

public class PlayerCam : MonoBehaviour {

	[SerializeField] private Transform m_cam;
	public CameraMode camMode = CameraMode.FREE;
	public Transform followTarget;									// The transform to follow
	[SerializeField] private Transform m_freeFocusTarget;			// The transform to face
	private Transform m_clashFocusTarget;
	[SerializeField] private float m_moveSpeed = 3;
	[SerializeField] private float m_turnSpeed = 1;
	[SerializeField] private float m_transitionMoveSpeed = 0.5f;
	[SerializeField] private float m_transitionFreezeTime = 0.75f;	// Duration after transition to freeze for dramatic effect
	[SerializeField] private float m_transitionTime = 3;
	private Vector3 m_freePos;										// Position for FREE rig
	private Quaternion m_freeRotation;								// Rotation for FREE rig
	private Vector3 m_clashPos;										// Position for CLASH rig
	private Quaternion m_clashRotation;								// Rotation for CLASH rig
	[SerializeField] private Transform m_clashRotationRef;			// Reference to transform used for setting m_clashRotation
	[SerializeField] private float m_clashExtraHeight;				// Extra height added during clash to give more view behind players


	private void Start() {
		m_freePos = transform.position;
		m_freeRotation = transform.rotation;
		m_clashPos = m_clashRotationRef.position;
		m_clashRotation = m_clashRotationRef.rotation;
		// Rotate to account for the camera offset
		m_clashRotation *= Quaternion.Euler(-m_cam.eulerAngles.x, 0, 0);
	}

	private void FixedUpdate()
	{
		if (camMode == CameraMode.FREE) {
			FreeCam(Time.deltaTime, m_moveSpeed);
		} else if (camMode == CameraMode.CLASH) {
			ClashCam(Time.deltaTime, m_moveSpeed);
		}
	}

	private void FreeCam(float deltaTime, float speed)
	{
		if (!(deltaTime > 0) || followTarget == null)
		{
			return;
		}
		transform.position = Vector3.Lerp(transform.position, followTarget.position, deltaTime * speed);
		var rollRotation = Quaternion.LookRotation(m_freeFocusTarget.position - followTarget.position, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, rollRotation, deltaTime * speed * m_turnSpeed);
	}

	private void ClashCam(float deltaTime, float speed)
	{
		if (!(deltaTime > 0) || followTarget == null)
		{
			return;
		}
		Vector3 midpoint = GetMidpointOnGround(followTarget.position, m_clashFocusTarget.position);
		float playersDist = Vector2.Distance(new Vector2(followTarget.position.x, followTarget.position.z), new Vector2(m_clashFocusTarget.position.x, m_clashFocusTarget.position.z));
		// Lerp camera toward midpoint of attacker and defender and increase height enough to allow sufficient vision
		transform.position = Vector3.Lerp(transform.position, new Vector3(midpoint.x, playersDist + m_clashExtraHeight, midpoint.z), speed * deltaTime);
		KeepAttackerOnLeft(deltaTime, speed);
	}

	private void KeepAttackerOnLeft(float deltaTime, float speed) {
		// Align rotation ref to two players
		m_clashRotationRef.right = m_clashFocusTarget.position - followTarget.position;
		// Reset m_clashRotationRef's x and z to its original value; we only want to rotate y
		m_clashRotationRef.rotation = Quaternion.Euler(new Vector3(m_clashRotation.eulerAngles.x, m_clashRotationRef.eulerAngles.y, m_clashRotationRef.eulerAngles.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, m_clashRotationRef.rotation, speed * deltaTime);
	}

	private Vector3 GetMidpointOnGround(Vector3 a, Vector3 b) {
		return new Vector3((a.x + b.x) / 2, 0, (a.z + b.z) / 2);
	}

	public IEnumerator TransitionToClash(Transform defender) {
		m_clashFocusTarget = defender;
		camMode = CameraMode.TRANSITIONING;
		float timeElapsed = 0;
		while (timeElapsed < m_transitionTime + m_transitionFreezeTime) {
			timeElapsed += Time.fixedDeltaTime;
			ClashCam(Time.deltaTime, m_transitionMoveSpeed);
			yield return new WaitForEndOfFrame();
		}
		camMode = CameraMode.CLASH;
	}

	public IEnumerator TransitionToFree() {
		camMode = CameraMode.TRANSITIONING;
		float timeElapsed = 0;
		while (timeElapsed < m_transitionTime + m_transitionFreezeTime) {
			FreeCam(Time.deltaTime, m_transitionMoveSpeed);
			timeElapsed += Time.fixedDeltaTime;
			yield return new WaitForEndOfFrame();
		}
		camMode = CameraMode.FREE;
	}
}
