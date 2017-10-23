using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShoot : MonoBehaviour {

	private Camera fpsCam;
	public float fireRate = .01f;
	public float range = 20f;
	private WaitForSeconds duration = new WaitForSeconds (.07f);
	public Transform point;
	private LineRenderer laserLine;
	private float nextFire;


	// Use this for initialization
	void Start () {
		laserLine = GetComponent<LineRenderer> ();
		fpsCam = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		//&& Time.time > nextFire
		if (Input.GetButtonDown ("Fire1") ) {
			nextFire = Time.time + fireRate;
			StartCoroutine (ShotEffect());
			Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
			RaycastHit hit;

			laserLine.SetPosition (0, point.position);

			if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, range)) {
				laserLine.SetPosition (1, hit.point);
				PlaySound box = hit.collider.GetComponent<PlaySound> ();
				if(box != null){
					box.strike ();
					Debug.Log ("Target exists");
				}
			} else {
				laserLine.SetPosition (1, rayOrigin + (fpsCam.transform.forward * range));
			}
		}
	}

	private IEnumerator ShotEffect(){
		laserLine.enabled = true;
		yield return duration;
		laserLine.enabled = false;
	}
}
