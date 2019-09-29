using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour {
    public string keyName = "Jump";
    public float dashMultiplier = 5f;
    public float dashTime = 1f;

    Beyblade b;
    Rigidbody rb;
    GroundCheck ground;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        ground = GetComponent<GroundCheck>();
        b = GetComponent<Beyblade>();
	}
	
	// Update is called once per frame
	void Update () {
        if (ground.isGrounded && Input.GetAxis(keyName) != 0)
        {
            StartCoroutine("Dash");
        }
	}

    IEnumerator Dash()
    {
        // Start of Dash
        rb.velocity = rb.velocity * dashMultiplier;
        b.DisablePlayerInfluence();
        yield return new WaitForSeconds(dashTime);
        b.EnablePlayerInfluence();
    }
}
