using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour, PlayerControls {
    public string keyName = "Jump";
    public float dashMultiplier = 5f;
    public float dashTime = 1f;

    Beyblade b;
    Rigidbody rb;
    GroundCheck ground;
	BeybladeSFX sfx;
	
	bool ready = true;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        ground = GetComponent<GroundCheck>();
        b = GetComponent<Beyblade>();
		sfx = b.GetComponent<BeybladeSFX>();
	}
	
	// Update is called once per frame
	void Update () {
        if (ready && ground.isGrounded && Input.GetAxis(keyName) != 0)
        {
            StartCoroutine("Dash");
        }
	}
	
	public void DisablePlayerInfluence(){
		ready = false;
	}
	
	public void EnablePlayerInfluence(){
		ready = true;
	}

    IEnumerator Dash()
    {
        // Start of Dash
		sfx.PlayAudio(1, 0.5f);
        rb.velocity = rb.velocity * dashMultiplier;
        b.DisablePlayerInfluence();
        yield return new WaitForSeconds(dashTime);
        b.EnablePlayerInfluence();
    }
}
