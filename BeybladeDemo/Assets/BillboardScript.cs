using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardScript : MonoBehaviour {
	public Camera followCam;
     void Update() {
         transform.rotation = followCam.transform.rotation;
     }
}
