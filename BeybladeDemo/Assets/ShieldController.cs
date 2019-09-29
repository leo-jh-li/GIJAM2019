using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour {

	public bool canUseShield = false;
	public float rotationSpeed;
	public Transform shieldPivot;
	void Start () {
		/// Testing :
		canUseShield = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!canUseShield) return;

		if (Input.GetKey(KeyCode.I)) {
			shieldPivot.Rotate(new Vector3(0, -rotationSpeed * Time.deltaTime, 0));
		} else if (Input.GetKey(KeyCode.O)) {
			shieldPivot.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
		}
	}
}
