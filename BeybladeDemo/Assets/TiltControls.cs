using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TiltControls : MonoBehaviour {

	public float m_maxTilt = 20f;
	public float m_smooth = 5f;

  public Transform center;
  public Transform sphere;

  int layerMask;

	float sigmoid(float x, float x_offset) {
		return (float)(1 / (1 + Math.Exp((double)(-x + x_offset))));
	}

	// Use this for initialization
	void Start () {
		layerMask = LayerMask.GetMask("floor");
	}
	
	// Update is called once per frame
	void Update () {
		float x_tilt = 0;
		float z_tilt = 0;

		// Work on Forward / Backwards Tilt
		if(Input.GetKey("w")) {
			x_tilt = m_maxTilt * sigmoid(m_maxTilt/m_maxTilt * 6, 5);
		}
		else if(Input.GetKey("s")) {
			x_tilt = -1 * m_maxTilt * sigmoid(m_maxTilt/m_maxTilt * 6, 5);
		}

		// Work on Left / Right Tilt
		if(Input.GetKey("a")) {
			z_tilt = m_maxTilt * sigmoid(m_maxTilt/m_maxTilt * 6, 5);
		}
		else if(Input.GetKey("d")) {
			z_tilt = -1 * m_maxTilt * sigmoid(m_maxTilt/m_maxTilt * 6, 5);
		}

    Vector3 requestedTilt = new Vector3(x_tilt,  transform.rotation.y, z_tilt);

    RaycastHit hit;
    Vector3 downDirection = center.transform.TransformDirection(Vector3.down);
    Debug.DrawLine(center.transform.position, downDirection);
    if (Physics.Raycast(center.transform.position, downDirection, out hit, 999999f, layerMask)) {
      sphere.transform.position = hit.point;
      Quaternion baseRotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(90, 0, 0);
      Quaternion targetRotation = Quaternion.Euler(x_tilt, 0, z_tilt) * baseRotation;
      transform.rotation = Quaternion.Slerp(
        transform.rotation,
        targetRotation,
        Time.deltaTime * m_smooth);
    }
	}
}