using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShoot : MonoBehaviour {

	public float fireRate = .01f;
	public float range = 20f;
	private WaitForSeconds duration = new WaitForSeconds (.07f);
	public Transform point;
	private LineRenderer laserLine;
	private float nextFire;
	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device Controller
	{
		get {return SteamVR_Controller.Input ((int)trackedObj.index);}
	}

	void Awake(){
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		laserLine = GetComponent<LineRenderer> ();
	}

	// Update is called once per frame
	void Update () {
		//&& Time.time > nextFire
		if (Controller.GetHairTriggerDown () ) {
			nextFire = Time.time + fireRate;
			StartCoroutine (ShotEffect());
			Vector3 rayOrigin = point.position;
			RaycastHit hit;

			laserLine.SetPosition (0, point.position);

			if (Physics.Raycast (rayOrigin, point.forward, out hit, range)) {
				laserLine.SetPosition (1, hit.point);
				PlaySound box = hit.collider.GetComponent<PlaySound> ();
				if(box != null){
					box.strike ();
					Debug.Log ("Target exists");
				}
			} else {
				laserLine.SetPosition (1, rayOrigin + (point.forward * range));
			}
		}
	}

	private IEnumerator ShotEffect(){
		laserLine.enabled = true;
		yield return duration;
		laserLine.enabled = false;
	}
}
