using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	
	/* List sounds in order of insertion here:
	*  0. Collision
	*  1. Collision2
	*  2. Special move (can be ulti or clash)
	*/
	
	public AudioClip[] sounds;
	AudioSource src;

	// Use this for initialization
	void Start () {
		src = GetComponent<AudioSource>();
	}
	
	public void PlaySound(int index, float volume){
		src.PlayOneShot(sounds[index], volume);
	}
}
