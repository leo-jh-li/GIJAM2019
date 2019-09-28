using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControls : MonoBehaviour {

	public float m_maxSpeed = 5f;
	public float m_smooth = 5f;

	private Rigidbody rb;
    Vector3 vel;
    GroundCheck ground;

	// Use this for initialization
	void Start () {
		rb = GetComponentInChildren<Rigidbody>();
        ground = GetComponentInChildren<GroundCheck>();
        StartCoroutine("ApplyVelocity");
	}
	
	// Update is called once per frame
	void Update () {
        vel = new Vector3((Input.GetKey("d")? 1:0) - (Input.GetKey("a")? 1:0),
            0,
            (Input.GetKey("w") ? 1 : 0) - (Input.GetKey("s") ? 1 : 0)).normalized * m_maxSpeed;

        
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
