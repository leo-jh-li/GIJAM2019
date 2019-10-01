using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregameButtonMash : MonoBehaviour {

	[SerializeField] private GameObject[] players;
	[SerializeField] private MashControls[] mashControls;
	[SerializeField] private MashMeter[] mashMeters;
	[SerializeField] private float mashDuration;
	[SerializeField] private float topMashCount = 25;
	[SerializeField] private float staminaBonusPerMash = 10;
	[SerializeField] private float rotationBonusPerMash = 1;
	private bool mashingActive = false;
	private float mashStartTime;

	private void Start() {
		StartMashing();
	}

	public void StartMashing() {
		if (mashControls.Length != 2) {
			Debug.LogWarning("Only two players supported for pregame mashing.");
			if (mashControls.Length != mashMeters.Length) {
				Debug.LogWarning("There must be as many mash meters as players.");
			}
			return;
		}
		mashingActive = true;
		mashStartTime = Time.time;
	}

	private void Update() {
		if (mashingActive) {
			for (int i = 0; i < mashControls.Length; i++) {
				if (Input.GetButtonDown(mashControls[i].mashInput)) {
					mashControls[i].mashCount++;
					mashMeters[i].UpdateMeter(mashControls[i].mashCount / topMashCount);
					ApplyMashBonus(players[i]);
				}
			}
			if (Time.time - mashStartTime >= mashDuration) {
				mashingActive = false;
			}
		}
	}

	private void ApplyMashBonus(GameObject player) {
		player.GetComponent<Beyblade>().TakeDamage(-staminaBonusPerMash);
		player.GetComponentInChildren<Rotation>().m_rotationSpeed += rotationBonusPerMash;
	}
}
