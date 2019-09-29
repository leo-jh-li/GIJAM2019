using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeyCollisionEffects : MonoBehaviour {
	public float impactThresh;
	public ParticleSystem bottomSparksOneShot;
	public ParticleSystem bottomSparksLoop;
	public GroundCheck groundCheck;
	public TrailRenderer trailRenderer;
	public string collisionTag;


	Vector3 lastPos;
	public float movementThresh;
	public float maxTrailHeight = 5f;


	private int groundLayer;

	void Start() {
		groundLayer = LayerMask.GetMask("floor");
	}

	void Update() {
		if (Vector3.Distance(lastPos, transform.position) >= movementThresh) {
			bottomSparksLoop.Play();
		} else {
			bottomSparksLoop.Stop();
		}

		if (trailRenderer.emitting && !Physics.Raycast(transform.position + Vector3.up * 1f, Vector3.down, maxTrailHeight, groundLayer)) {
			trailRenderer.emitting = false;
		} else if (Physics.Raycast(transform.position +  Vector3.up * 1f, Vector3.down, maxTrailHeight, groundLayer) && !trailRenderer.emitting) {
			trailRenderer.emitting = true;
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
