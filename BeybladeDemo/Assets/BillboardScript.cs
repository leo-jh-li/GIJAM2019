using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardScript : MonoBehaviour {
	public Camera followCam;
     void Update() {
         if (followCam != null)
            transform.rotation = followCam.transform.rotation;
     }
}
