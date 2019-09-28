using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClashEvent : MonoBehaviour {
	//Players
	ClashEventModule m_attacker;
	ClashEventModule m_defender;

	//Attacker Target [L, R, U, D]
	List<Vector3> m_attackTargets;

	public void SetPlayers(ClashEventModule attacker, ClashEventModule defender) {
		m_attacker = attacker;
		m_defender = defender;
	}

	// Use this for initialization
	void Start () {
		//Init
		m_attackTargets = new List<Vector3>();

		//Compute Target points
		Vector3 collisionDir = m_defender.transform.position - m_attacker.transform.position;
		m_attackTargets.Add(m_defender.transform.position - collisionDir/2);
		m_attackTargets.Add(m_defender.transform.position + Quaternion.Euler(0, -90, 0) * collisionDir/2);
		m_attackTargets.Add(m_defender.transform.position + collisionDir/2);
		m_attackTargets.Add(m_defender.transform.position - Quaternion.Euler(0, -90, 0) * collisionDir/2);

		//Enable ClashEventModule
		m_attacker.enabled = true;
		m_defender.enabled = true;

		//Start the Coroutine
		StartCoroutine(Turn());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Turn() {
		while(true) {
			int attackerCommand = m_attacker.GetCommand();
			int defenderCommand = m_defender.GetCommand();
			yield return new WaitForSeconds(1f);	
		}
	}
}
