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
		if (playerInfluence) {
			if (controlMode == ControlMode.PERSPECTIVE) {
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
			} else if (controlMode == ControlMode.WORLD) {
				vel = new Vector3((Input.GetKey(right) ? 1:0) - (Input.GetKey(left) ? 1:0),
					0,
					(Input.GetKey(up) ? 1 : 0) - (Input.GetKey(down) ? 1 : 0));
			}
		}
		vel = vel.normalized * m_maxSpeed;
        vel = Vector3.ProjectOnPlane(vel, ground.groundNormal);
		transform.forward = Vector3.ProjectOnPlane(cameraRig.transform.forward, ground.groundNormal);
		Debug.DrawLine(transform.position, transform.position + transform.forward * 50, Color.red);
        //print(vel.ToString());
		// Work on Forward / Backwards Tilt

		rb.velocity = Vector3.Slerp(
			rb.velocity,
			vel,
			Time.deltaTime * m_smooth);

		//print(rb.velocity);
	}

	// void LateUpdate() {
	// 	transform.forward = cameraRig.transform.forward;
	// }
}
