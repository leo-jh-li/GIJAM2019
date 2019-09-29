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

	// Cameras
	public CamerasManager m_cam;

	public Canvas player1Canvas;
	public Canvas player2Canvas;

	// Initiate 2D Mode
	public void Initiate2D(Beyblade attacker, Beyblade defender, int comboCount) {
		if (!m_gameMode) {
			StartCoroutine(FreezeFrame2D(attacker, defender, comboCount));
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

			player1Canvas.worldCamera = m_cam.getMyCamera(p1.gameObject);
			player2Canvas.worldCamera = m_cam.getMyCamera(p2.gameObject);
			
		}
	}

	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator FreezeFrame2D(Beyblade attacker, Beyblade defender, int comboCount) {
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

		//Start the event
		m_cevent.enabled = true;
	}

	IEnumerator FreezeFrame3D(Beyblade p1, Beyblade p2, Vector3 bounceBack) {
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
