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
	private string[] sounds;
	private int sound_pg_offset;

	// Use this for initialization
	void Start () {

		sound_menu.SetActive (false);
		trackedObj = GetComponent<SteamVR_TrackedObject> ();

		sounds = new string[6];
		sounds [0] = "Piano";
		sounds [1] = "Drum";
		sounds [2] = "Trumpet";
		sounds [3] = "Piano2";
		sounds [4] = "Drum2";
		sounds [5] = "Trumpet2";

		sound_pg_offset = 0;
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Controller.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
			Debug.Log (Controller.GetAxis ().x + " , " + Controller.GetAxis ().y);
			if (Controller.GetAxis ().y < -.7) {
				sound_menu.SetActive (!sound_menu.activeSelf);
			}

			if (Controller.GetAxis ().x < -.7) {
				sound_pg_offset = Mathf.Max (sound_pg_offset - 3, 0);
				flipPage ();
			}

			if (Controller.GetAxis ().x > .7) {
				sound_pg_offset = Mathf.Min (sound_pg_offset + 3, sounds.Length-3);
				flipPage ();
			}
		}

	}

	void flipPage(){
		
		// set text
		if (sound_menu.activeSelf) {
			int numMenuItems = sound_menu.transform.childCount;
			for (int i = 0; i < numMenuItems; i++) {
				Transform menu_item = sound_menu.transform.GetChild (i);
				Transform menu_text_obj = menu_item.GetChild (0);
				TextMesh menu_text = menu_text_obj.GetComponentInParent<TextMesh> ();
				menu_text.text = sounds [sound_pg_offset + i];
			}
			//GameObject[] menu_items = sound_menu.transform.GetChild;
			//for (int i = 0; i < menu_items.Length; i++) {
			//	TextMesh sound_text = menu_items [i].GetComponentInChildren<TextMesh> ();
			//	sound_text.text = sounds [sound_pg_offset + i];
			//}
		}
	}

				
}
