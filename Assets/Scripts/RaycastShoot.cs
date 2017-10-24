using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShoot : MonoBehaviour {

	public float range = 20f;
	public Transform point;
	private LineRenderer laserLine;
	private PlaySound last;
	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device Controller
	{
		get {return SteamVR_Controller.Input ((int)trackedObj.index);}
	}

	void Start(){
		laserLine.enabled = false;
	}

	void Awake(){
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		laserLine = GetComponent<LineRenderer> ();
		laserLine.enabled = true;
	}

	// Update is called once per frame
	void Update () {
		Vector3 rayOrigin = point.position;
		RaycastHit hit;

		laserLine.SetPosition (0, point.position);

		if (Physics.Raycast (rayOrigin, point.forward, out hit, range)) {
			laserLine.SetPosition (1, hit.point);
			PlaySound box = hit.collider.GetComponent<PlaySound> ();
			if(box != null && Controller.GetHairTriggerDown()){
				box.strike ();
			}
		} else {
			laserLine.SetPosition (1, rayOrigin + (point.forward * range));
		}
		
	}
}
