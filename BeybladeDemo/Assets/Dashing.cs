using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour {

    public MonoBehaviour[] movementScripts;
    public string keyName = "Jump";
    public float dashMultiplier = 5f;
    public float dashTime = 1f;

    Rigidbody rb;
    GroundCheck ground;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        ground = GetComponent<GroundCheck>();
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
        foreach (MonoBehaviour s in movementScripts)
            s.enabled = false;
        this.enabled = false;
        yield return new WaitForSeconds(dashTime);
        // End of Dash
        foreach (MonoBehaviour s in movementScripts)
            s.enabled = true;
        this.enabled = true;
    }
}
