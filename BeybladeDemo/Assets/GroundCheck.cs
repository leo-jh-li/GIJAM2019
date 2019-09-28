using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

    /* For refactoring colliders in the future:
     * Put this and a collider on a separate child GameObject, so as to
     * avoid being called by another Collider
     */

    HashSet<Collider> groundedOn;

    public bool isGrounded = false;
    public Vector3 groundNormal = Vector3.up;

	// Use this for initialization
	void Start () {
        groundedOn = new HashSet<Collider>();
	}

    void OnCollisionEnter(Collision c)
    {
        groundedOn.Add(c.collider);
        isGrounded = true;

        groundNormal = c.contacts[0].normal;
    }

    void OnCollisionStay(Collision c)
    {
        groundNormal = c.contacts[0].normal;
    }

    void OnCollisionExit(Collision c)
    {
        groundedOn.Remove(c.collider);
        if (groundedOn.Count == 0)
            isGrounded = false;
    }
}
