using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2MovementControls : MonoBehaviour {

	public float m_maxSpeed = 5f;
	public float m_smooth = 5f;

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponentInChildren<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		float x_velocity = 0;
		float z_velocity = 0;

		// Work on Forward / Backwards Tilt
		if(Input.GetKey("up")) {
			z_velocity = m_maxSpeed;
		}
		else if(Input.GetKey("down")) {
			z_velocity = -1 * m_maxSpeed;
		}

		// Work on Left / Right Tilt
		if(Input.GetKey("left")) {
			x_velocity = -1 * m_maxSpeed;
		}
		else if(Input.GetKey("right")) {
			x_velocity = m_maxSpeed;
		}

		rb.velocity = Vector3.Slerp(
			rb.velocity,
			new Vector3(x_velocity, rb.velocity.y, z_velocity),
			Time.deltaTime * m_smooth);

		//print(rb.velocity);
	}
}
