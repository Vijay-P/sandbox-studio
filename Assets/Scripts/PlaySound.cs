using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour {

	AudioSource source;
	public AudioClip hitSound;
	public Material active;
	public Material inactive;
	private Renderer render;

	private WaitForSeconds duration = new WaitForSeconds (.07f);

	void Start(){
		source = GetComponent<AudioSource> ();
		source.spatialize = true;
		source.spatialBlend = 1.0f;
		source.rolloffMode = AudioRolloffMode.Custom;
		render = GetComponent<Renderer>();
		render.enabled = true;
		inactive = render.material;
	}

	public void strike(){
		source.PlayOneShot(hitSound, 1f);
		StartCoroutine (playEffect ());

	}

	private IEnumerator playEffect(){
		render.sharedMaterial = active;
		yield return duration;
		render.sharedMaterial = inactive;
	}
}
