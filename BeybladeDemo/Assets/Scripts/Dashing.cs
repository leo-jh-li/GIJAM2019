﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour, PlayerControls {
    public string keyName = "Jump";
    public string controllerName;
    public float dashMultiplier = 5f;
    public float dashTime = 1f;

    Beyblade b;
    Rigidbody rb;
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
        rb = GetComponent<Rigidbody>();
        ground = GetComponent<GroundCheck>();
        b = GetComponent<Beyblade>();
	}
	
	// Update is called once per frame
	void Update () {
        if (ground.isGrounded && (Input.GetAxis(keyName) != 0 || Input.GetAxis(controllerName) != 0) && playerInfluence)
        {
            StartCoroutine("Dash");
        }
	}

    IEnumerator Dash()
    {
        // Start of Dash
		b.PlaySound(2,0.7f);
        rb.velocity = rb.velocity * dashMultiplier;
        b.DisablePlayerInfluence();
        yield return new WaitForSeconds(dashTime);
        b.EnablePlayerInfluence();
    }
}
