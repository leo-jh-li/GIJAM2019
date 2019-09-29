using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregameButtonMash : MonoBehaviour {

	[SerializeField] private MashControls[] mashControls;
	[SerializeField] private MashMeter[] mashMeters;
	[SerializeField] private float mashDuration;
	[SerializeField] private float topMashCount = 30;

	public void StartMashing() {
		if (mashControls.Length != 2) {
			Debug.LogWarning("Only two players supported for pregame mashing.");
			if (mashControls.Length != mashMeters.Length) {
				Debug.LogWarning("There must be as many mash meters as players.");
			}
		}
	}

	private IEnumerator OpenMashingWindow() {
		float timeElapsed = 0;
		while (timeElapsed < mashDuration) {
			for (int i = 0; i < mashControls.Length; i++) {
				if (Input.GetButtonDown(mashControls[i].mashInput)) {
					mashControls[i].mashCount++;
					mashMeters[i].UpdateMeter(mashControls[i].mashCount / topMashCount);
				}
			}
			yield return new WaitForEndOfFrame();
		}
	}
}
