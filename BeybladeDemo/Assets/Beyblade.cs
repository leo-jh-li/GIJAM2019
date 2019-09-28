using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beyblade : MonoBehaviour {
	
	public float m_maxStamina;
	float m_stamina;

	// Based on Shielding [1, m_defenseMultiplier]
	public float m_defenseMultiplier;
	float m_currentDefenseMultiplier;

	// Based on Velocity [0, m_attackMultiplier]
	public float m_attackMultiplier;

	MovementControls m_mc;

	float ComputeCollisionResult(BeybladePiece thisPiece, BeybladePiece otherPiece) {
		Beyblade otherBey = otherPiece.m_parent;

		return GetAttackMultiplier() * thisPiece.m_attack + GetDefenseMultiplier() * thisPiece.m_defense 
			- (otherPiece.m_attack * otherBey.GetAttackMultiplier() + otherPiece.m_defense * otherBey.GetDefenseMultiplier());
	}

	public float GetDefenseMultiplier() {
		return m_currentDefenseMultiplier;
	}

	public float GetAttackMultiplier() {
		float curSpeed = GetComponent<Rigidbody>().velocity.magnitude;
		float maxSpeed = m_mc.m_maxSpeed;
		return m_attackMultiplier * Utilities.sigmoid((curSpeed/maxSpeed) * 6, 6);
	}

	public void TakeDamage(float dmg) {
		m_stamina = (m_stamina - dmg < 0) ? 0 : m_stamina;
	}

	public void BeybladeCollision(BeybladePiece thisPiece, BeybladePiece otherPiece) {
		float result = ComputeCollisionResult(thisPiece, otherPiece);
		if(result > 0f) {
			print("Triggering 2d");
		}
		else {
			TakeDamage(-1 * result);
		}
	}

	void Start() {
		m_mc = GetComponent<MovementControls>();
		m_stamina = m_maxStamina;		
	}

	void Update() {

	}
}