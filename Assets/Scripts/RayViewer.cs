using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayViewer : MonoBehaviour {

	public float range = 20f;
	public Camera fpsCam;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 lineOrigin = fpsCam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0));
		Debug.DrawRay (lineOrigin, fpsCam.transform.forward * range, Color.green);
	}
}
