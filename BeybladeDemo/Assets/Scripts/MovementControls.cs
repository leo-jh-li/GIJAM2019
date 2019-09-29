using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControls : MonoBehaviour, PlayerControls {

	public float m_maxSpeed = 5f;
	public float m_smooth = 5f;

	private Rigidbody rb;
    Vector3 vel;
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
		vel = Vector3.zero;
		playerInfluence = true;
		rb = GetComponentInChildren<Rigidbody>();
        ground = GetComponentInChildren<GroundCheck>();
        StartCoroutine("ApplyVelocity");
	}
	
	// Update is called once per frame
	void Update () {
        vel = new Vector3((Input.GetKey(right) && playerInfluence ? 1:0) - (Input.GetKey(left) && playerInfluence ? 1:0),
            0,
            (Input.GetKey(up) && playerInfluence ? 1 : 0) - (Input.GetKey(down) && playerInfluence ? 1 : 0)).normalized * m_maxSpeed;

        
        //print(vel.ToString());
		// Work on Forward / Backwards Tilt


		//print(rb.velocity);
	}

    IEnumerator ApplyVelocity()
    {
        while (true)
        {
            rb.velocity = Vector3.Slerp(
            rb.velocity,
            Vector3.ProjectOnPlane(vel, ground.groundNormal),
            Time.deltaTime * m_smooth);
            yield return null;
        }
    }
}
