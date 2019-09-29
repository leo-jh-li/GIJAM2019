using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSkills : MonoBehaviour, PlayerControls {
	public float m_currentUlt = 0f;
	public string fire;
	public float m_animationTime;
	[SerializeField]UltimateCollider m_ultimateCollider;

	Beyblade player;

	bool playerInfluence;

	private System.Action<float> callback;

    //Disable/Enable Player Controls without disabling physics
    public void DisablePlayerInfluence()
    {
    	playerInfluence = false;
    }

	public void EnablePlayerInfluence()
	{
		playerInfluence = true;
	}

	public void ChargeUltimateBar(float value) {
		m_currentUlt = m_currentUlt + value >= 100 ? 100 : m_currentUlt + value;
		if (this.callback != null) {
			this.callback.Invoke(m_currentUlt);
		}
	}

	public void UseUltimate() {
		StartCoroutine(ActivateUltimate());
	}

	void Start() {
		playerInfluence = true;
		player = GetComponent<Beyblade>();
		this.ChargeUltimateBar(0);
		//m_ultimateCollider = GetComponentInChildren<UltimateCollider>();
	}

	void Update() {
		if(Input.GetButtonDown(fire) && playerInfluence && m_currentUlt == 100) {
			UseUltimate();
			m_currentUlt = 0;
		}
	}

	IEnumerator ActivateUltimate() {
		player.m_gameSystem.FreezeGameState();
		player.m_gameSystem.NoLight();
		yield return new WaitForSeconds(m_animationTime);

		Beyblade otherPlayer = player.m_gameSystem.p1 == player ? player.m_gameSystem.p2 : player.m_gameSystem.p1;

		//Enable Ultimate collider
		otherPlayer.EnableBeybladePieces();
		m_ultimateCollider.gameObject.SetActive(true);

		//Let collision handle the rest
		LeanTween.move( gameObject, otherPlayer.gameObject.transform, 0.5f).setEase( LeanTweenType.easeInQuad ).setDelay(1f);
	}

	public void setUltimateCallback(System.Action<float> callback) {
		this.callback = callback;
	}
}