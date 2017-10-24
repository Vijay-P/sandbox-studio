﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeMenuController : MonoBehaviour {

	public GameObject menu;
	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	// On TrackPad 

	private void Controller_PadClicked(object sender, ClickedEventArgs e){
		if (Controller.GetAxis().x != 0 || Controller.GetAxis().y != 0) {
			Debug.Log (Controller.GetAxis().x + " , " + Controller.GetAxis().y);

		}
	}

	// Use this for initialization
	void Start () {
		menu.SetActive (false);
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Controller.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
			Debug.Log (Controller.GetAxis ().x + " , " + Controller.GetAxis ().y);
			if (Controller.GetAxis ().y > .7) {
				menu.SetActive (!menu.activeSelf);
			}
		}
		
	}
}
