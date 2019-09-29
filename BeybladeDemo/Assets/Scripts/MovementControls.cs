using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControls : MonoBehaviour, PlayerControls {

	public float m_maxSpeed = 5f;
	public float m_smooth = 5f;

	private Rigidbody rb;

	// Keyboard input names
    public string up, down, left, right;
	// Controller input names
    public string horizontal, vertical;

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
		if(ground.isGrounded && playerInfluence) {
			// Keyboard input
			vel = new Vector3((Input.GetKey(right) ? 1:0) - (Input.GetKey(left) ? 1:0),
				0,
				(Input.GetKey(up) ? 1 : 0) - (Input.GetKey(down) ? 1 : 0));
			// Controller input (has priority over keyboard)
			Vector3 controllerVel = new Vector3(Input.GetAxis(horizontal),
				0,
				Input.GetAxis(vertical));
				if(controllerVel!=Vector3.zero){
					vel = controllerVel;
				}
		}
		vel = Vector3.ProjectOnPlane(vel.normalized * m_maxSpeed, ground.groundNormal);
		//print(vel.ToString());
		// Work on Forward / Backwards Tilt

		rb.velocity = Vector3.Slerp(
			rb.velocity,
			vel,
			Time.deltaTime * m_smooth);

		//print(rb.velocity);
	}
}
