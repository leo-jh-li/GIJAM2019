using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour {

	[SerializeField] private Transform m_followTarget;	// The transform to follow
	[SerializeField] private Transform m_focusTarget;	// The transform to face
	[SerializeField] private float m_moveSpeed = 3;
	[SerializeField] private float m_turnSpeed = 1;

	private void FixedUpdate()
	{
		MoveCam(Time.deltaTime);
	}

	private void MoveCam(float deltaTime)
	{
		if (!(deltaTime > 0) || m_followTarget == null)
		{
			return;
		}
		transform.position = Vector3.Lerp(transform.position, m_followTarget.position, deltaTime * m_moveSpeed);
		var rollRotation = Quaternion.LookRotation(m_focusTarget.position - m_followTarget.position, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, rollRotation, m_turnSpeed * deltaTime);
	}
}
