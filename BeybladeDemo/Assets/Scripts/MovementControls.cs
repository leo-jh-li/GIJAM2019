using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlMode {
	PERSPECTIVE,
	WORLD
}

public class MovementControls : MonoBehaviour, PlayerControls {

	public Transform cameraRig;
	public ControlMode controlMode;

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
		Vector3 controllerVel = Vector3.zero;
		if(ground.isGrounded && playerInfluence) {
			if (controlMode == ControlMode.PERSPECTIVE) {
				// Keyboard input
				if (Input.GetKey(up)) {
					vel += transform.forward;
				}
				if (Input.GetKey(down)) {
					vel -= transform.forward;
				}
				if (Input.GetKey(right)) {
					vel += transform.right;
				}
				if (Input.GetKey(left)) {
					vel -= transform.right;
				}
				// Controller input not set

			} else if (controlMode == ControlMode.WORLD) {
				// Keyboard input
				vel = new Vector3((Input.GetKey(right) ? 1:0) - (Input.GetKey(left) ? 1:0),
					0,
					(Input.GetKey(up) ? 1 : 0) - (Input.GetKey(down) ? 1 : 0));
				// Controller input (has priority over keyboard)
				controllerVel = new Vector3(Input.GetAxis(horizontal),
					0,
					Input.GetAxis(vertical));
			}

			if(controllerVel != Vector3.zero){
				vel = controllerVel;
			}
		}
		vel = Vector3.ProjectOnPlane(vel.normalized * m_maxSpeed, ground.groundNormal);
		if (controlMode == ControlMode.PERSPECTIVE) {
			transform.forward = Vector3.ProjectOnPlane(cameraRig.transform.forward, ground.groundNormal);
		} else if (controlMode == ControlMode.WORLD) {
			transform.forward = Vector3.forward;
		}

		rb.velocity = Vector3.Slerp(
			rb.velocity,
			vel,
			Time.deltaTime * m_smooth);
	}
}
