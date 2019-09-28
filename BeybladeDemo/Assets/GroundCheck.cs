using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

    CapsuleCollider groundCollider;

    HashSet<Collider> groundedOn;

    public bool isGrounded = false;

	// Use this for initialization
	void Start () {
        groundCollider = GetComponent<CapsuleCollider>();
        groundedOn = new HashSet<Collider>();
	}
	
	void OnTriggerEnter(Collider c)
    {
        groundedOn.Add(c);
        isGrounded = true;
    }

    void OnTriggerExit(Collider c)
    {
        groundedOn.Remove(c);
        if (groundedOn.Count == 0)
            isGrounded = false;
    }
}
