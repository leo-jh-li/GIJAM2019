using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour {

	//(0, 3D), (1, 2D)
	public bool m_gameMode;

	//EventSystems
	public ClashEvent m_cevent;

	//Delay Transition Modes
	public float m_delayTransition;

	//Players
	public Beyblade p1, p2;

	// Cameras
	public CamerasManager m_cam;

	//Directional Light
	public Light m_dirLight;

	public Canvas player1Canvas;
	public Canvas player2Canvas;

	public void OnDestroy() {
		StopAllCoroutines();
	}

	// Initiate 2D Mode
	public void Initiate2D(Beyblade attacker, Beyblade defender, int comboCount, float clashSpeed = 0f) {
		if (!m_gameMode) {
			StartCoroutine(FreezeFrame2D(attacker, defender, comboCount, clashSpeed));
			m_gameMode = !m_gameMode;

			player1Canvas.worldCamera = m_cam.getMyCamera(attacker.gameObject);
			player2Canvas.worldCamera = m_cam.getMyCamera(attacker.gameObject);
		}
	}

	public void Initiate3D(Beyblade p1, Beyblade p2, Vector3 bounceBack) {
		if(m_gameMode) {
			//Enable Controls
			StartCoroutine(FreezeFrame3D(p1, p2, bounceBack));
			m_gameMode = !m_gameMode;
			Camera cam1 = m_cam.getMyCamera(p1.gameObject);
			Camera cam2 = m_cam.getMyCamera(p2.gameObject);
			if (cam1 == m_cam.m_leftCam) {
				player1Canvas.worldCamera = cam1;
				player2Canvas.worldCamera = cam2;
			} else {
				player1Canvas.worldCamera = cam2;
				player2Canvas.worldCamera = cam1;
			}
		}
	}

	
	// Update is called once per frame
	void Update () {
		
	}

	public void FreezeGameState() {
		//Disable Pieces
        p1.DisableBeybladePieces();
        p2.DisableBeybladePieces();

        //Disable Controls
        p1.DisablePlayerInfluence();
        p2.DisablePlayerInfluence();

        //Set Velocity to 0
		p1.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
		p2.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
	}

	public void NoLight() {
		m_dirLight.intensity = 0f;
	}

	public void ResetLight() {
		m_dirLight.intensity = 1f;
	}

	IEnumerator FreezeFrame2D(Beyblade attacker, Beyblade defender, int comboCount, float clashSpeed) {
		//Freeze physics
		attacker.GetComponentInChildren<MovementControls>().enabled = false;
		attacker.GetComponentInChildren<TiltControls>().enabled = false;

		defender.GetComponentInChildren<MovementControls>().enabled = false;
		defender.GetComponentInChildren<TiltControls>().enabled = false;

		Vector3 a_velo = attacker.GetComponentInChildren<Rigidbody>().velocity;
		Vector3 d_velo = defender.GetComponentInChildren<Rigidbody>().velocity;

		//Set Velocity to 0
		attacker.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
		defender.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;

		//Initialize
		m_cevent.SetPlayers(attacker.GetComponent<ClashEventModule>(), defender.GetComponent<ClashEventModule>());
		attacker.GetComponent<ClashEventModule>().enabled = true;
		defender.GetComponent<ClashEventModule>().enabled = true;

		//Disable Pieces
        attacker.DisableBeybladePieces();
        defender.DisableBeybladePieces();

        //Focused Camera
        m_cam.FocusMyCam(attacker.gameObject);

		yield return new WaitForSeconds(m_delayTransition / 2);

		attacker.BounceBack(defender, a_velo - d_velo);
		yield return new WaitForSeconds(m_delayTransition / 2);
		
		//Set Velocity to 0
		attacker.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
		defender.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;

		//Unfreeze physics
		attacker.GetComponentInChildren<MovementControls>().enabled = true;
		attacker.GetComponentInChildren<TiltControls>().enabled = true;

		defender.GetComponentInChildren<MovementControls>().enabled = true;
		defender.GetComponentInChildren<TiltControls>().enabled = true;

        //Disable Controls
        attacker.DisablePlayerInfluence();
        defender.DisablePlayerInfluence();

		//Designate Combo Count
		m_cevent.SetMaxCombo(comboCount);

		//Enable Shield
		defender.GetComponent<ShieldController>().enabled = true;

		//Set the Clash Speed
		if(clashSpeed > 0f) {
			m_cevent.SetAttackDuration(clashSpeed);	
		}

		//Start the event
		m_cevent.enabled = true;
	}

	IEnumerator FreezeFrame3D(Beyblade p1, Beyblade p2, Vector3 bounceBack) {
		ResetLight();

		p1.GetComponent<ClashEventModule>().enabled = false;
		p2.GetComponent<ClashEventModule>().enabled = false;

		//Unfreeze physics
		p1.GetComponentInChildren<MovementControls>().enabled = true;
		p1.GetComponentInChildren<TiltControls>().enabled = true;

		p2.GetComponentInChildren<MovementControls>().enabled = true;
		p2.GetComponentInChildren<TiltControls>().enabled = true;

		//TODO: Hard Coded Value
		p1.BounceBack(p2, bounceBack, 200);

		//UnFocus Camera
		m_cam.ResetCameras();

		//Disable Shield
		p1.GetComponent<ShieldController>().enabled = false;
		p2.GetComponent<ShieldController>().enabled = false;

		yield return new WaitForSeconds(m_delayTransition);

		//Enable Pieces
        p1.EnableBeybladePieces();
        p2.EnableBeybladePieces();

        //Enable Controls
        p1.EnablePlayerInfluence();
        p2.EnablePlayerInfluence();

        //Reset Collision
        p1.ResetCollision();
        p2.ResetCollision();
	}
}
