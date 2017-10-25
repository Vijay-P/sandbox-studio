using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundContainer : MonoBehaviour {

	public AudioClip sound;

	// Use this for initialization
	void Start () {
		if (sound != null)
			gameObject.GetComponentInChildren<TextMesh> ().text = sound.name;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
