using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTexture : MonoBehaviour {

	public float scaleFactor;
	private Renderer render;

	// Use this for initialization
	void Start () {
		render = GetComponent<Renderer>();
		render.material.SetTextureScale("_MainTex", new Vector2(scaleFactor, scaleFactor));
	}

	// Update is called once per frame
	void Update () {
		
	}
}
