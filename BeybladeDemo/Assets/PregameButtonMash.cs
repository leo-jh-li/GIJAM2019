using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregameButtonMash : MonoBehaviour {

	[SerializeField] private GameObject[] players;
	[SerializeField] private MashControls[] mashControls;
	[SerializeField] private MashMeter[] mashMeters;
	[SerializeField] private float mashDuration;
	[SerializeField] private float topMashCount = 30;
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
					IncreaseSpeed(players[i]);
				}
			}
			if (Time.time - mashStartTime >= mashDuration) {
				mashingActive = false;
			}
		}
	}

	private void IncreaseSpeed(GameObject player) {
		player.GetComponent<Beyblade>().m_stamina += 20;
		player.GetComponentInChildren<Rotation>().m_rotationSpeed += 1;
	}
}
