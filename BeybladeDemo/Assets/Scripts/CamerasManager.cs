using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasManager : MonoBehaviour {

	[SerializeField] private PlayerCam m_leftPlayerCam;
	[SerializeField] private Camera m_leftCam;
	[SerializeField] private Vector4 m_leftDefaultRect;
	[SerializeField] private PlayerCam m_rightPlayerCam;
	[SerializeField] private Camera m_rightCam;
	[SerializeField] private Vector4 m_rightDefaultRect;
	[SerializeField] private Vector4 m_fullCamRect;
	[SerializeField] private float m_transitionSpeed;

	[Header("Particles")]
	[SerializeField] private ParticleSystem m_leftSideSparks;
	private GameObject m_leftSideParticles;
	[SerializeField] private ParticleSystem m_rightSideSparks;
	private GameObject m_rightSideParticles;


	private void Start () {
		// Set split screen
		m_leftCam.rect = new Rect(m_leftDefaultRect.x, m_leftDefaultRect.y, m_leftDefaultRect.z, m_leftDefaultRect.w);
		m_rightCam.rect = new Rect(m_rightDefaultRect.x, m_rightDefaultRect.y, m_rightDefaultRect.z, m_rightDefaultRect.w);
		m_leftSideParticles = m_leftSideSparks.transform.parent.gameObject;
		m_rightSideParticles = m_rightSideSparks.transform.parent.gameObject;
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
		while (m_leftCam.rect.width < m_fullCamRect.z) {
			m_leftCam.rect = new Rect(m_leftDefaultRect.x, m_leftDefaultRect.y, Mathf.Lerp(m_leftCam.rect.width, m_fullCamRect.z, timeElapsed * m_transitionSpeed), m_leftDefaultRect.w);
			m_rightCam.rect = new Rect(Mathf.Lerp(m_rightCam.rect.x, 1, timeElapsed * m_transitionSpeed), m_rightDefaultRect.y, Mathf.Lerp(m_rightCam.rect.width, 0, timeElapsed * m_transitionSpeed), m_rightDefaultRect.w);
			// m_rightSideParticles.position = new ;
			timeElapsed += Time.fixedDeltaTime;
			yield return new WaitForEndOfFrame();
		}
	}

	public IEnumerator FocusRightCam() {
		float timeElapsed = 0;
		StartCoroutine(m_rightPlayerCam.TransitionToClash(m_leftPlayerCam.followTarget));
		while (m_rightCam.rect.width < m_fullCamRect.z) {
			m_rightCam.rect = new Rect(Mathf.Lerp(m_rightCam.rect.x, m_fullCamRect.x, timeElapsed * m_transitionSpeed), m_rightDefaultRect.y, Mathf.Lerp(m_rightCam.rect.width, m_fullCamRect.z, timeElapsed * m_transitionSpeed), m_rightDefaultRect.w);
			m_leftCam.rect = new Rect(m_leftDefaultRect.x, m_leftDefaultRect.y, Mathf.Lerp(m_leftCam.rect.width, 0, timeElapsed * m_transitionSpeed), m_leftDefaultRect.w);
			timeElapsed += Time.fixedDeltaTime;
			yield return new WaitForEndOfFrame();
		}
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
