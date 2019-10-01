using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TiltControls : MonoBehaviour, PlayerControls {

	public float m_maxTilt = 20f;
	public float m_smooth = 5f;

	public Transform center;

	public string up, down, left, right;
	
	bool playerInfluence;
	int layerMask;

	//Disable/Enable Player Controls without disabling physics
    public void DisablePlayerInfluence()
    {
    	playerInfluence = false;
    }

	public void EnablePlayerInfluence()
	{
		playerInfluence = true;
	}

	// Use this for initialization
	void Start () {
		playerInfluence = true;
		layerMask = LayerMask.GetMask("floor");
	}

	// Update is called once per frame
	void Update () {
		float x_tilt = 0;
		float z_tilt = 0;

		// Work on Forward / Backwards Tilt
		if(Input.GetKey(up) && playerInfluence) {
			x_tilt = m_maxTilt * Utilities.sigmoid(m_maxTilt/m_maxTilt * 6, 5);
		}
		else if(Input.GetKey(down) && playerInfluence) {
			x_tilt = -1 * m_maxTilt * Utilities.sigmoid(m_maxTilt/m_maxTilt * 6, 5);
		}

		// Work on Left / Right Tilt
		if(Input.GetKey(left) && playerInfluence) {
			z_tilt = m_maxTilt * Utilities.sigmoid(m_maxTilt/m_maxTilt * 6, 5);
		}
		else if(Input.GetKey(right) && playerInfluence) {
			z_tilt = -1 * m_maxTilt * Utilities.sigmoid(m_maxTilt/m_maxTilt * 6, 5);
		}

		RaycastHit hit;
		Vector3 downDirection = center.transform.TransformDirection(Vector3.down);

		// Debug.DrawLine(center.transform.position, downDirection);
		if (Physics.Raycast(center.transform.position, downDirection, out hit, 999999f, layerMask)) {
			// float forwardRotationY = SignedAngleBetween(transform.parent.forward, Vector3.forward, Vector3.up);
			float forwardRotationY = Vector3.SignedAngle(transform.parent.forward, Vector3.forward, Vector3.up);
			Vector3 rotatedTiltAngles = (Quaternion.Euler(x_tilt, 0, z_tilt) * Quaternion.Euler(0, forwardRotationY, 0)).eulerAngles;
			Quaternion baseRotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(90, 0, 0);
			Quaternion targetRotation = Quaternion.Euler(rotatedTiltAngles.x, 0, rotatedTiltAngles.z) * baseRotation;

			transform.rotation = Quaternion.Slerp(
			transform.rotation,
			targetRotation,
			Time.deltaTime * m_smooth);
		}
	}
}