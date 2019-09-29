using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Beyblade : MonoBehaviour, PlayerControls {
	public MonoBehaviour[] movementScripts;
	public GameObject[] m_beybladePieces;
	public GameSystem m_gameSystem;
	public float m_maxStamina = 100f;
	[SerializeField]float m_stamina;

	// Based on Shielding [1, m_defenseMultiplier]
	public float m_defenseMultiplier;
	float m_currentDefenseMultiplier;

	// Based on Velocity [0, m_attackMultiplier]
	public float m_attackMultiplier;

	//Clash Event Thresholds
	public float m_ceventDamageThreshold = 1f;
	public float m_ceventVelocityThreshold = 1f;
	public float m_bounceDisableTime = 1f;

	public Animator deathAnimator;

	// Bitmap For Beyblade Collision
	bool m_collision;

	// To Compute Damage
	MovementControls m_mc;

	private System.Action<float> uiHealthCallback;
	public int playerIndex = 0;
	public ParticleSystem playerExplosion;

	public float deathAnimTime = 3f;

	public Transform BeyVisuals;
	public UltimateSkillUI ultiSkillUi;

	float ComputeCollisionResult(BeybladePiece thisPiece, BeybladePiece otherPiece) {
		Beyblade otherBey = otherPiece.m_parent;

		return GetAttackMultiplier() * thisPiece.m_attack + GetDefenseMultiplier() * thisPiece.m_defense 
			- (otherPiece.m_attack * otherBey.GetAttackMultiplier() + otherPiece.m_defense * otherBey.GetDefenseMultiplier());
	}

	bool ClashEventDecision(Beyblade other, float collisionResult) {
		return (collisionResult >= m_ceventDamageThreshold || 
			GetComponent<Rigidbody>().velocity.magnitude + other.GetComponent<Rigidbody>().velocity.magnitude >= m_ceventVelocityThreshold) 
			&& collisionResult > 0f;
	}

	void BounceBack(Beyblade other, float collisionResult) {
		Vector3 collisionDir = (GetComponent<Rigidbody>().velocity - other.GetComponent<Rigidbody>().velocity).normalized;

		//TODO: FIX HARDCODED MINIMUM
		Vector3 bounceBack = m_mc.m_maxSpeed * Utilities.sigmoid(collisionResult, 4) * collisionDir;
		if (bounceBack.magnitude < 60) {
			bounceBack = 60 * collisionDir;
		}
		GetComponent<Rigidbody>().velocity = -1 * bounceBack;
		other.GetComponent<Rigidbody>().velocity = bounceBack;
	}

	public void BounceBack(Beyblade other, Vector3 dir, float magnitude = 60) {
		dir = dir.normalized * magnitude;
		GetComponent<Rigidbody>().velocity = -1 * dir;
		other.GetComponent<Rigidbody>().velocity = dir;
	}

	//Disable/Enable Beyblade Part Colliders
	public void DisableBeybladePieces() {
		foreach (GameObject bp in m_beybladePieces) {
			bp.SetActive(false);
		}
	}

	public void EnableBeybladePieces() {
		foreach (GameObject bp in m_beybladePieces) {
			bp.SetActive(true);
		}
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
		return m_currentDefenseMultiplier;
	}

	public float GetAttackMultiplier() {
		float curSpeed = GetComponent<Rigidbody>().velocity.magnitude;
		float maxSpeed = m_mc.m_maxSpeed;
		return m_attackMultiplier * Utilities.sigmoid((curSpeed/maxSpeed) * 6, 6);
	}

	public void TakeDamage(float dmg) {
		Debug.Log("m_Stamina: " + m_stamina + " " + m_maxStamina + " " + playerIndex);
		if (this.enabled == false) return;

		m_stamina = (m_stamina - dmg < 0) ? 0 : m_stamina - dmg;
		if (uiHealthCallback != null) {
			uiHealthCallback.Invoke(m_stamina);
		}
		if (m_stamina <= 0) {
			this.Die();
		}
	}

	public void Die() {
		this.DisablePlayerInfluence();
		this.deathAnimator.enabled = true;
		this.deathAnimator.Play("DeathAnim");
		StartCoroutine(deathAnimCoroutine());
	}

	IEnumerator deathAnimCoroutine() {
		float counter = 0;
		while (counter <= deathAnimTime) {
			counter += Time.deltaTime;
			yield return null;
		}

		SceneManager.LoadScene("MainMenu");

	}

	public void PlayExplosion() {
		this.BeyVisuals.gameObject.SetActive(false);
		this.playerExplosion.Play();
	}

	public void BeybladeCollision(BeybladePiece thisPiece, BeybladePiece otherPiece) {
		float result = ComputeCollisionResult(thisPiece, otherPiece);
		print(result + gameObject.name);
		if(ClashEventDecision(otherPiece.m_parent, result) && !m_collision) {
			otherPiece.m_parent.TakeDamage(4 * result);
			print(thisPiece.gameObject.name + " " + gameObject.name + " -> " + otherPiece.gameObject.name + " " + otherPiece.m_parent.name);
			//TODO: Determine combo count
			m_gameSystem.Initiate2D(this, otherPiece.m_parent, 10);
		}
		else if(!m_collision && result > 0f) {
			DisablePlayerInfluence();
			Invoke("EnablePlayerInfluence", m_bounceDisableTime);
			BounceBack(otherPiece.m_parent, result);
			otherPiece.m_parent.TakeDamage(result);
		}
		m_collision = true;
	}

	public void UltimateBeybladeCollision(UltimateCollider thisPiece, BeybladePiece otherPiece) {
		thisPiece.gameObject.SetActive(false);
		this.ultiSkillUi.doUltimate();
		m_gameSystem.Initiate2D(this, otherPiece.m_parent, 40, 0.1f);
	}

	public void ResetCollision() {
		m_collision = false;
	}

	public void setHealthUICallback(System.Action<float> callback) {
		this.uiHealthCallback = callback;
	}

	void Start() {
		m_mc = GetComponent<MovementControls>();
		m_stamina = m_maxStamina;	
		this.TakeDamage(0);
		m_collision = false;
		this.deathAnimator.enabled = false;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Q)) {
			this.TakeDamage(1023);
		}
	}
}