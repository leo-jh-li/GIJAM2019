using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasManager : MonoBehaviour {

	[SerializeField] private Camera m_leftCam;
	[SerializeField] private Vector4 m_leftCamRect;
	[SerializeField] private Camera m_rightCam;
	[SerializeField] private Vector4 m_rightCamRect;
	[SerializeField] private Vector4 m_fullCamRect;

	private void Start () {
		// Set split screen
		m_leftCam.rect = new Rect(m_leftCamRect.x, m_leftCamRect.y, m_leftCamRect.z, m_leftCamRect.w);
		m_rightCam.rect = new Rect(m_rightCamRect.x, m_rightCamRect.y, m_rightCamRect.z, m_rightCamRect.w);
	}
}
