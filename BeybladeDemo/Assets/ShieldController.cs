using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour {

	public float rotationSpeed;
	public Transform shieldPivot;

	public ClashEventModule defender;

	void OnEnable () {
		shieldPivot.gameObject.SetActive(true);
	}
	
	void OnDisable() {
		shieldPivot.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		if (defender.GetCommand() == 0) {
			shieldPivot.Rotate(new Vector3(0, -rotationSpeed * Time.deltaTime, 0));
		} else if (defender.GetCommand() == 2) {
			shieldPivot.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
		}
		defender.ResetCommand();
	}
}
