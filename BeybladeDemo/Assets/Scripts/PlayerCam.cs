using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode
{
	FREE,			// 3D
	TRANSITIONING,	// Transitioning for 3D to 2D or 2D to 3D
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
	[SerializeField] private float m_transitionFreezeTime = 0.75f;		// Duration after transition to freeze for cool effect
	[SerializeField] private float m_transitionTime = 3;
	[SerializeField] private Vector3 m_freePos;						// Camera position for FREE camera
	private Quaternion m_freeRotation;									// Euler angles for FREE camera
	[SerializeField] private Vector3 m_clashPos;					// Camera position for CLASH camera
	[SerializeField] private Vector3 m_clashEulers;					// Euler angles for CLASH camera

	private void Start() {
		m_freeRotation = m_cam.rotation;
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
		transform.rotation = Quaternion.Lerp(transform.rotation, rollRotation, speed * deltaTime);
	}

	private void ClashCam(float deltaTime, float speed)
	{
		if (!(deltaTime > 0) || followTarget == null)
		{
			return;
		}
		Vector3 midpoint = GetMidpointOnGround(followTarget.position, m_clashFocusTarget.position);
		Vector2 perpendicularVect = Vector2.Perpendicular(new Vector2(m_clashFocusTarget.position.x - followTarget.position.x, m_clashFocusTarget.position.z - followTarget.position.z));
		/*
		Vector3 lookAt = new Vector3(perpendicularVect.x, midpoint.y, perpendicularVect.y);
		transform.rotation = Quaternion.LookRotation(lookAt);
		transform.position = Vector3.Lerp(transform.position, midpoint, speed * deltaTime);
		*/
		// Lerp camera toward midpoint of attacker and defender
		m_cam.position = Vector3.Lerp(m_cam.position, new Vector3(midpoint.x, m_cam.position.y, midpoint.z), speed * deltaTime);		
	}


	private Vector3 GetMidpointOnGround(Vector3 a, Vector3 b) {
		return new Vector3((a.x + b.x) / 2, 0, (a.z + b.z) / 2);
	}

	public IEnumerator TransitionToClash(Transform defender) {
		m_clashFocusTarget = defender;
		camMode = CameraMode.TRANSITIONING;
		float timeElapsed = 0;
		while (timeElapsed < m_transitionTime) {
			Vector3 midpoint = GetMidpointOnGround(followTarget.position, m_clashFocusTarget.position);
			timeElapsed += Time.fixedDeltaTime;
			m_cam.position = Vector3.Lerp(m_cam.position, new Vector3(midpoint.x, m_clashPos.y, midpoint.z), timeElapsed / m_transitionTime);
			m_cam.eulerAngles = Vector3.Lerp(m_cam.eulerAngles, m_clashEulers, timeElapsed / m_transitionTime);
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSeconds(m_transitionFreezeTime);
		camMode = CameraMode.CLASH;
	}

	public IEnumerator TransitionToFree() {
		camMode = CameraMode.TRANSITIONING;
		float timeElapsed = 0;
		while (timeElapsed < m_transitionTime) {
			timeElapsed += Time.fixedDeltaTime;
			m_cam.localPosition = Vector3.Lerp(m_cam.localPosition, m_freePos, timeElapsed / m_transitionTime);
			// m_cam.eulerAngles = Vector3.Lerp(m_cam.eulerAngles, m_freeEulers, timeElapsed / m_transitionTime);
			// m_cam.rotation = Quaternion.Slerp(m_cam.rotation, m_freeRotation, timeElapsed / m_transitionTime); 
			yield return new WaitForEndOfFrame();
		}
		m_cam.rotation = m_freeRotation;
		yield return new WaitForSeconds(m_transitionFreezeTime);
		camMode = CameraMode.FREE;
	}
}
