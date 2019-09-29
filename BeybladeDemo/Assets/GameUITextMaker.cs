using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUITextMaker : MonoBehaviour {

    public GameObject damageTextPrefab;

	public Transform testTransform;
	
    public void createText(Vector3 position, float comboDamage) {

        // Instantiate the text.
        Debug.Log("Create text!");
        GameObject textObject = Instantiate(this.damageTextPrefab, position, Quaternion.identity);
        FadingText fadeScript = textObject.GetComponent<FadingText>();
        fadeScript.StartFade(comboDamage);
        //textObject.GetComponent<BillboardScript>().followCam = isAttacker ? Camera.main : Camera.main; //TODO: Change
    }

	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.M)) {
            this.createText(testTransform.position, 30);
        }
	}
}
