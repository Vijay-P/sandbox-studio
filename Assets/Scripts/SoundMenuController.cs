using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMenuController : MonoBehaviour {

	public GameObject sound_menu;
	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	// Use this for initialization
	void Start () {

		sound_menu.SetActive (false);
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Controller.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
			Debug.Log (Controller.GetAxis ().x + " , " + Controller.GetAxis ().y);
			if (Controller.GetAxis ().y < -.7) {
				sound_menu.SetActive (!sound_menu.activeSelf);
			}
		}

		
	}
}
