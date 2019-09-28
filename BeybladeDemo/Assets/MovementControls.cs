using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControls : MonoBehaviour {

	public float m_maxSpeed = 5f;
	public float m_smooth = 5f;

	private Rigidbody rb;
    GroundCheck ground;

	// Use this for initialization
	void Start () {
		rb = GetComponentInChildren<Rigidbody>();
        ground = GetComponentInChildren<GroundCheck>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 vel = new Vector3((Input.GetKey("d")? 1:0) - (Input.GetKey("a")? 1:0),
            0,
            (Input.GetKey("w") ? 1 : 0) - (Input.GetKey("s") ? 1 : 0)).normalized * m_maxSpeed;

        vel = Vector3.ProjectOnPlane(vel, ground.groundNormal);
        //print(vel.ToString());
		// Work on Forward / Backwards Tilt

		rb.velocity = Vector3.Slerp(
			rb.velocity,
			vel,
			Time.deltaTime * m_smooth);

		//print(rb.velocity);
	}
}
