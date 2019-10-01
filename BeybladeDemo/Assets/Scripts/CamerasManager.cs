using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasManager : MonoBehaviour {

	[SerializeField] private PlayerCam m_leftPlayerCam;
	public Camera m_leftCam;
	[SerializeField] private Vector4 m_leftDefaultRect;
	[SerializeField] private PlayerCam m_rightPlayerCam;
	public Camera m_rightCam;
	[SerializeField] private Vector4 m_rightDefaultRect;
	[SerializeField] private Vector4 m_fullCamRect;
	[SerializeField] private float m_transitionSpeed;

	[Header("Particles")]
	[SerializeField] private Transform particlesCanvas;
	private float canvasWidth;
	[SerializeField] private ParticleSystem m_leftSideSparks;
	private RectTransform m_leftSideRt;
	[SerializeField] private ParticleSystem m_rightSideSparks;
	private RectTransform m_rightSideRt;

	public Camera getMyCamera(GameObject player) {
		if (m_leftPlayerCam.followTarget.gameObject == player) {
			return m_leftCam;
		} else {
		return m_rightCam;
		}
	}

	private void Start () {
		// Set split screen
		m_leftCam.rect = new Rect(m_leftDefaultRect.x, m_leftDefaultRect.y, m_leftDefaultRect.z, m_leftDefaultRect.w);
		m_rightCam.rect = new Rect(m_rightDefaultRect.x, m_rightDefaultRect.y, m_rightDefaultRect.z, m_rightDefaultRect.w);
		canvasWidth = particlesCanvas.GetComponent<RectTransform>().rect.width;
		m_leftSideRt = m_leftSideSparks.transform.parent.gameObject.GetComponent<RectTransform>();
		m_rightSideRt = m_rightSideSparks.transform.parent.gameObject.GetComponent<RectTransform>();
	}

	public IEnumerator ResetToSplitScreen() {
		float timeElapsed = 0;
		StartCoroutine(m_leftPlayerCam.TransitionToFree());
		StartCoroutine(m_rightPlayerCam.TransitionToFree());
		while (m_leftCam.rect.width != m_leftDefaultRect.z) {
			m_leftCam.rect = new Rect(m_leftDefaultRect.x, m_leftDefaultRect.y, Mathf.Lerp(m_leftCam.rect.width, m_leftDefaultRect.z, timeElapsed * m_transitionSpeed), m_leftDefaultRect.w);
			m_rightCam.rect = new Rect(Mathf.Lerp(m_rightCam.rect.x, m_rightDefaultRect.x, timeElapsed * m_transitionSpeed), m_rightDefaultRect.y, Mathf.Lerp(m_rightCam.rect.width, m_rightDefaultRect.z, timeElapsed * m_transitionSpeed), m_rightDefaultRect.w);
			timeElapsed += Time.fixedDeltaTime;
			yield return new WaitForEndOfFrame();
		}
	}

	public IEnumerator FocusLeftCam() {
		float timeElapsed = 0;
		StartCoroutine(m_leftPlayerCam.TransitionToClash(m_rightPlayerCam.followTarget));
		m_rightSideSparks.Play();
		m_rightSideRt.anchoredPosition = new Vector2(-canvasWidth / 2, m_rightSideRt.anchoredPosition.y);
		while (m_leftCam.rect.width < m_fullCamRect.z) {
			m_leftCam.rect = new Rect(m_leftDefaultRect.x, m_leftDefaultRect.y, Mathf.Lerp(m_leftCam.rect.width, m_fullCamRect.z, timeElapsed * m_transitionSpeed), m_leftDefaultRect.w);
			m_rightCam.rect = new Rect(Mathf.Lerp(m_rightCam.rect.x, 1, timeElapsed * m_transitionSpeed), m_rightDefaultRect.y, Mathf.Lerp(m_rightCam.rect.width, 0, timeElapsed * m_transitionSpeed), m_rightDefaultRect.w);
			m_rightSideRt.anchoredPosition = new Vector2(Mathf.Lerp(m_rightSideRt.anchoredPosition.x, 0, timeElapsed * m_transitionSpeed), m_rightSideRt.anchoredPosition.y);
			timeElapsed += Time.fixedDeltaTime;
			yield return new WaitForEndOfFrame();
		}
	}

	public IEnumerator FocusRightCam() {
		float timeElapsed = 0;
		StartCoroutine(m_rightPlayerCam.TransitionToClash(m_leftPlayerCam.followTarget));
		m_leftSideSparks.Play();
		m_leftSideRt.anchoredPosition = new Vector2(canvasWidth / 2, m_leftSideRt.anchoredPosition.y);
		while (m_rightCam.rect.width < m_fullCamRect.z) {
			m_rightCam.rect = new Rect(Mathf.Lerp(m_rightCam.rect.x, m_fullCamRect.x, timeElapsed * m_transitionSpeed), m_rightDefaultRect.y, Mathf.Lerp(m_rightCam.rect.width, m_fullCamRect.z, timeElapsed * m_transitionSpeed), m_rightDefaultRect.w);
			m_leftCam.rect = new Rect(m_leftDefaultRect.x, m_leftDefaultRect.y, Mathf.Lerp(m_leftCam.rect.width, 0, timeElapsed * m_transitionSpeed), m_leftDefaultRect.w);
			m_leftSideRt.anchoredPosition = new Vector2(Mathf.Lerp(m_leftSideRt.anchoredPosition.x, 0, timeElapsed * m_transitionSpeed), m_leftSideRt.anchoredPosition.y);
			timeElapsed += Time.fixedDeltaTime;
			yield return new WaitForEndOfFrame();
		}
	}

	public void FocusMyCam(GameObject follower) {
		if(m_leftPlayerCam.followTarget.gameObject == follower) {
			StartCoroutine(FocusLeftCam());
		}
		else {
			StartCoroutine(FocusRightCam());
		}
	}

	public void ResetCameras() {
		StartCoroutine(ResetToSplitScreen());
	}

	// Buttons for testing
	void Update() {
		if (Input.GetKeyDown(KeyCode.Keypad0)) {
			StartCoroutine(ResetToSplitScreen());
		}
		if (Input.GetKeyDown(KeyCode.Keypad1)) {
			StartCoroutine(FocusLeftCam());
		}
		if (Input.GetKeyDown(KeyCode.Keypad2)) {
			StartCoroutine(FocusRightCam());
		}
	}
}
