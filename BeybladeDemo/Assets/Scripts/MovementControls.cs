using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControls : MonoBehaviour, PlayerControls {

	public float m_maxSpeed = 5f;
	public float m_smooth = 5f;

	private Rigidbody rb;

    public string up, down, left, right;

    GroundCheck ground;
    bool playerInfluence;

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
		rb = GetComponentInChildren<Rigidbody>();
        ground = GetComponentInChildren<GroundCheck>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 vel = Vector3.zero;
		if(ground.isGrounded) {
        	vel = new Vector3((Input.GetKey(right) && playerInfluence ? 1:0) - (Input.GetKey(left) && playerInfluence ? 1:0),
            	0,
            	(Input.GetKey(up) && playerInfluence ? 1 : 0) - (Input.GetKey(down) && playerInfluence ? 1 : 0)).normalized * m_maxSpeed;
		}

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
