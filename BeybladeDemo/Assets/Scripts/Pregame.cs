using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pregame : MonoBehaviour {

	[SerializeField] private TMPro.TextMeshProUGUI m_countdownText;
	[SerializeField] private int m_countdownLength = 3;
	[SerializeField] private string m_startMessage = "GO";				// Feel free to change to let it rip
	[SerializeField] private float m_startMessageStartZoom = 0.5f;		// Starting scale of the start message. Needs to be smaller for longer messages
	[SerializeField] private float m_startMessageEndZoom = 5;
	[SerializeField] private float m_startMessageZoomDuration = 0.75f;

	private void Start () {
		StartCoroutine(Countdown());
	}

	private IEnumerator Countdown() {
		m_countdownText.gameObject.SetActive(true);
		for (int i = m_countdownLength; i > 0; i--) {
			m_countdownText.text = i.ToString();
			yield return new WaitForSeconds(1f);
		}
		StartCoroutine(GoMessage());
	}

	private IEnumerator GoMessage() {
		m_countdownText.text = m_startMessage;
		float timeElapsed = 0;
		m_countdownText.gameObject.GetComponent<RectTransform>().localScale = new Vector2(m_startMessageStartZoom, m_startMessageStartZoom);
		float scale = m_startMessageStartZoom;
		while (timeElapsed < m_startMessageZoomDuration) {
			timeElapsed += Time.deltaTime;
			scale = Mathf.Lerp(scale, m_startMessageEndZoom, timeElapsed);
			m_countdownText.gameObject.GetComponent<RectTransform>().localScale = new Vector2(scale, scale);
			yield return new WaitForEndOfFrame();
		}
		gameObject.SetActive(false);
	}
}
