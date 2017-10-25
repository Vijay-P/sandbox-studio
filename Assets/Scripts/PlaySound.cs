using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour {

	AudioSource source;
	public AudioClip hitSound;
	public Material active;
	private Material inactive;
	private Renderer render;

	private WaitForSeconds duration = new WaitForSeconds (.07f);

	void Start(){
		source = GetComponent<AudioSource> ();
		render = GetComponent<Renderer>();
		render.enabled = true;
	}

	public void strike(){
		source.PlayOneShot(hitSound, 1f);
		StartCoroutine (playEffect ());
	}

	private IEnumerator playEffect(){
		inactive = render.material;
		render.sharedMaterial = active;
		yield return duration;
		render.sharedMaterial = inactive;
	}
}
