using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour {

	//(0, 3D), (1, 2D)
	public bool m_gameMode;

	//EventSystems
	public ClashEvent m_cevent;

	// Initiate 2D Mode
	public void Initiate2D(Beyblade attacker, Beyblade defender) {
		if (!m_gameMode) {
			//Disable Controls
			attacker.GetComponentInChildren<MovementControls>().enabled = false;
			attacker.GetComponentInChildren<TiltControls>().enabled = false;

			defender.GetComponentInChildren<MovementControls>().enabled = false;
			defender.GetComponentInChildren<TiltControls>().enabled = false;

			//Set Velocity to 0
			attacker.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
			defender.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;

			//Start the event
			m_cevent.SetPlayers(attacker.GetComponent<ClashEventModule>(), defender.GetComponent<ClashEventModule>());
			m_cevent.enabled = true;
			m_gameMode = !m_gameMode;
		}
	}

	public void Initiate3D(Beyblade p1, Beyblade p2) {
		if(m_gameMode) {
			//Enable Controls
			p1.GetComponentInChildren<MovementControls>().enabled = true;
			p1.GetComponentInChildren<TiltControls>().enabled = true;

			p2.GetComponentInChildren<MovementControls>().enabled = true;
			p2.GetComponentInChildren<TiltControls>().enabled = true;
			m_gameMode = !m_gameMode;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
