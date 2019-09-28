using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour {

	//(0, 3D), (1, 2D)
	public bool m_gameMode;

	//Players
	public Beyblade m_p1;
	public Beyblade m_p2;

	//EventSystems
	public ClashEvent m_cevent;

	// Initiate 2D Mode
	public void Initiate2D(Beyblade attacker, Beyblade defender) {
		if (!m_gameMode) {
			//Disable Controls
			m_p1.GetComponentInChildren<MovementControls>().enabled = false;
			m_p1.GetComponentInChildren<TiltControls>().enabled = false;

			m_p2.GetComponentInChildren<MovementControls>().enabled = false;
			m_p2.GetComponentInChildren<TiltControls>().enabled = false;

			//Set Velocity to 0
			m_p1.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
			m_p2.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;

			//Start the event
			m_cevent.SetPlayers(attacker.GetComponent<ClashEventModule>(), defender.GetComponent<ClashEventModule>());
			m_gameMode = !m_gameMode;
		}
	}

	public void Initiate3D() {
		if(m_gameMode) {
			//Enable Controls
			m_p1.GetComponentInChildren<MovementControls>().enabled = true;
			m_p1.GetComponentInChildren<TiltControls>().enabled = true;

			m_p2.GetComponentInChildren<MovementControls>().enabled = true;
			m_p2.GetComponentInChildren<TiltControls>().enabled = true;
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
