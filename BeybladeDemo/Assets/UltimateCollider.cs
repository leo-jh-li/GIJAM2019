using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateCollider : MonoBehaviour {

	public Beyblade m_parent;

	public virtual void CollideWithPiece(BeybladePiece other)
	{
		m_parent.UltimateBeybladeCollision(this, other);
	}

	void OnTriggerEnter(Collider other) {
		BeybladePiece piece = other.GetComponent<BeybladePiece>();
		if(piece) {
			CollideWithPiece(piece);
		}
	}
}