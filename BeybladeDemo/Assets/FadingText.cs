using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingText : MonoBehaviour {

	public TMPro.TextMeshPro texMesh;
	public float textFadeSpeed = 1.0f;
	public float textDisplayTime = 1.0f;

	public void StartFade(float val) {
		texMesh.text = val.ToString();
		StartCoroutine(fadeCor());
	}
	
	IEnumerator fadeCor() {
		float counter = 0;
		while (counter <= 1) {
			texMesh.color = new Color(texMesh.color.r, texMesh.color.g, texMesh.color.b, counter);
			counter += textFadeSpeed * Time.deltaTime;
			yield return null;
		}

		yield return new WaitForSeconds(textDisplayTime);
		counter = 0;
		while (counter <= 1) {
			texMesh.color = new Color(texMesh.color.r, texMesh.color.g, texMesh.color.b, 1- counter);
			counter += textFadeSpeed * Time.deltaTime;
			yield return null;
		}

		texMesh.color = new Color(texMesh.color.r, texMesh.color.g, texMesh.color.b, 0);
		Destroy(this.gameObject);
	}
}
