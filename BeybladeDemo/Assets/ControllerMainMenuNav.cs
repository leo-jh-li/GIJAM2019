using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerMainMenuNav : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		// TODO: change buttons!
		if (Input.GetKeyDown(KeyCode.A)) {
			SceneManager.LoadScene("gameScene");
		} else if (Input.GetKeyDown(KeyCode.S)) {
			SceneManager.LoadScene("Instructions");
		}
	}
}
