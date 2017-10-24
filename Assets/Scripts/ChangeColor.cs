using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour {
	public Material Red;
	public Material Green;
	Renderer rend;



	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();
		rend.enabled = true;
		Red = Resources.Load("Materials/Red", typeof(Material)) as Material;
		Green = Resources.Load("Materials/Green", typeof(Material)) as Material;
		rend.sharedMaterial = Red;
	}
	
	void OnCollisionEnter(Collision col){

		if (col.gameObject.tag == "box") {
			rend.sharedMaterial = Green;
		} else {
			rend.sharedMaterial = Red;
		}
	}
}
