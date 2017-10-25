using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInstructions : MonoBehaviour {

	public GameObject instructions;

	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}
	private bool instr_status;

	// Use this for initialization
	void Start () {
		instr_status = true;
		instructions.SetActive (instr_status);
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Controller.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
			instr_status = !instr_status;
			instructions.SetActive (instr_status);
		}
		
	}
}
