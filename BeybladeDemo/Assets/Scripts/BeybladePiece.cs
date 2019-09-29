using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeybladePiece : MonoBehaviour {

	public float m_attack;
	public float m_defense;
	public Collider m_collider;
	public Beyblade m_parent;
	public BeybladeSFX sfx;

	public virtual void CollideWithPiece(BeybladePiece other)
	{
		m_parent.BeybladeCollision(this, other);
	}

	void OnTriggerEnter(Collider other) {
		BeybladePiece piece = other.GetComponent<BeybladePiece>();
		if(piece) {
			sfx.PlayAudio(0,1);
			CollideWithPiece(piece);
		}
	}

	void OnTriggerExit(Collider other) {
		BeybladePiece piece = other.GetComponent<BeybladePiece>();
		if(piece) {
			m_parent.ResetCollision();
		}	
	}
}
