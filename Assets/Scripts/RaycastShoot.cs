using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShoot : MonoBehaviour {

	public float range = 20f;
	public Transform point;
	public Stolen_Teleporter teleporter;
	public Stolen_Laser tpointer;
	private LineRenderer laserLine;
	private PlaySound last;
	private SteamVR_TrackedObject trackedObj;
	private bool canFire;
	private SteamVR_Controller.Device Controller
	{
		get {return SteamVR_Controller.Input ((int)trackedObj.index);}
	}
		
	void Awake(){
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		laserLine = GetComponent<LineRenderer> ();
		laserLine.enabled = false;
		canFire = true;
		last = null;
	}

	// Update is called once per frame
	void Update () {
		Vector3 rayOrigin = point.position;
		RaycastHit hit;

		laserLine.SetPosition (0, point.position);
		if (laserLine.enabled && Physics.Raycast (rayOrigin, point.forward, out hit, range)) {
			laserLine.SetPosition (1, hit.point);
			PlaySound box = hit.collider.GetComponent<PlaySound> ();
			if (box != null && last == null) {
				box.strike ();
				Controller.TriggerHapticPulse (3500);
				last = box;
			} else if(box == null) {
				last = null;
			}
		} else {
			laserLine.SetPosition (1, rayOrigin + (point.forward * range));
			last = null;
		}

		if (Controller.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {
			Debug.Log ("Grip Pressed");
			laserLine.enabled = !laserLine.enabled;
			if (tpointer != null && teleporter != null) {
				teleporter.enabled = !teleporter.enabled;
				tpointer.enabled = !tpointer.enabled;
			}
		}
	}
}
