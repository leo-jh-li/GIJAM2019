using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeybladeSFX : MonoBehaviour {
	
	public AudioClip[] sounds;
	/* Specify sounds in order here:
	*  0: Hitting
	*  1: Dashing
	*/
	public AudioSource oneShot;

	public void PlayAudio(int sound, float vol){
		oneShot.PlayOneShot(sounds[sound], vol);
	}
}
