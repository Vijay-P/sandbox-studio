using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour {

	AudioSource source;
	public AudioClip hitSound;

	void Start(){
		source = GetComponent<AudioSource> ();
	}

	public void strike(){
		source.PlayOneShot(hitSound, 1f);
		Debug.Log ("Function Called");
	}
}
