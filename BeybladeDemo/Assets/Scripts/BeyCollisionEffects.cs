using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeyCollisionEffects : MonoBehaviour {
	public float impactThresh;
	public ParticleSystem bottomSparksOneShot;
	public ParticleSystem bottomSparksLoop;

	public string collisionTag;


	Vector3 lastPos;
	public float movementThresh;

	void Start() {
	}

	void Update() {
		Debug.Log(Vector3.Distance(lastPos, transform.position));
		if (Vector3.Distance(lastPos, transform.position) >= movementThresh) {
			bottomSparksLoop.Play();
		} else {
			bottomSparksLoop.Stop();
		}

		lastPos = transform.position;
	}
	void OnCollisionEnter(Collision other) {
		float impactForce = Vector3.Magnitude(other.relativeVelocity);
		if (other.gameObject.tag == collisionTag && impactForce >= impactThresh) {
			bottomSparksOneShot.Play();
		}
	}
}
