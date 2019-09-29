using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSkillUI : MonoBehaviour {

	public CanvasGroup group;
	public float stayTime = 1f;
	public float fadeInTime = 0.25f;


	void Start() {
		group.alpha = 0;
	}

	public void doUltimate() {
		StartCoroutine(ultimateCor());
	}

	IEnumerator ultimateCor() {
		float counter = 0;
		group.alpha = 0;
		while (counter < fadeInTime) {
			group.alpha = counter / fadeInTime;
			counter += Time.deltaTime;
			yield return null;
		}

		yield return new WaitForSeconds(stayTime);

		counter = 0;
		while (counter < fadeInTime) {
			group.alpha = 1 - (counter / fadeInTime);
			counter += Time.deltaTime;
			yield return null;
		}
		group.alpha = 0;


	}
}
