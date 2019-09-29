using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beyblade : MonoBehaviour, PlayerControls {
	public MonoBehaviour[] movementScripts;
	public GameSystem m_gameSystem;
	public float m_maxStamina;
	float m_stamina;

	// Based on Shielding [1, m_defenseMultiplier]
	public float m_defenseMultiplier;

	// Based on Velocity [0, m_attackMultiplier]
	public float m_attackMultiplier;

	//Clash Event Thresholds
	public float m_ceventDamageThreshold = 1f;
	public float m_ceventVelocityThreshold = 1f;
	public float m_bounceDisableTime = 1f;

	// Bitmap For Beyblade Collision
	bool m_collision;

	// To Compute Damage
	MovementControls m_mc;

	float ComputeCollisionResult(BeybladePiece thisPiece, BeybladePiece otherPiece) {
		Beyblade otherBey = otherPiece.m_parent;

		return GetAttackMultiplier() * thisPiece.m_attack + GetDefenseMultiplier() * thisPiece.m_defense 
			- (otherPiece.m_attack * otherBey.GetAttackMultiplier() + otherPiece.m_defense * otherBey.GetDefenseMultiplier());
	}

	bool ClashEventDecision(Beyblade other, float collisionResult) {
		return (collisionResult >= m_ceventDamageThreshold || 
			GetComponent<Rigidbody>().velocity.magnitude >= m_ceventVelocityThreshold ||
			other.GetComponent<Rigidbody>().velocity.magnitude >= m_ceventVelocityThreshold);
	}

	void BounceBack(Beyblade other, float collisionResult) {
		Vector3 collisionDir = GetComponent<Rigidbody>().velocity - other.GetComponent<Rigidbody>().velocity.normalized;
		DisablePlayerInfluence();
		Invoke("EnablePlayerInfluence", m_bounceDisableTime);
		GetComponent<Rigidbody>().velocity = -1 * m_mc.m_maxSpeed * Utilities.sigmoid(collisionResult, 4) * collisionDir / 3;
		other.GetComponent<Rigidbody>().velocity = m_mc.m_maxSpeed * Utilities.sigmoid(collisionResult, 4) * collisionDir / 3;
	}

	//Disable/Enable Player Controls without disabling physics
	public void DisablePlayerInfluence() {
		foreach (PlayerControls s in movementScripts)
            s.DisablePlayerInfluence();
	}

	public void EnablePlayerInfluence() {
		foreach (PlayerControls s in movementScripts)
            s.EnablePlayerInfluence();
	}

	public float GetDefenseMultiplier() {
		return m_defenseMultiplier;
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
		if(ClashEventDecision(otherPiece.m_parent, result) && !m_collision) {
			m_gameSystem.Initiate2D(this, otherPiece.m_parent);
		}
		else if(!m_collision && result > 0f) {
			BounceBack(otherPiece.m_parent, result);
		}	
		if(result < 0f) {
			print("Ouch! Took - " + result);
			TakeDamage(-1 * result);
		}

		m_collision = true;
	}

	public void ResetCollision() {
		m_collision = false;
	}

	void Start() {
		m_mc = GetComponent<MovementControls>();
		m_stamina = m_maxStamina;		
		m_collision = false;
	}

	void Update() {

	}
}