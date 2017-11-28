using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeTransformer : MonoBehaviour {

	public GameObject shapeMenu;
	public GameObject soundMenu;
	private SteamVR_TrackedObject trackedObj;
	private LineRenderer laserLine;
	private bool enableTransform;
	public bool grabbed;
	public GameObject grab;
	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	void Awake()
	{
		laserLine = GetComponent<LineRenderer> ();
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	// Use this for initialization
	void Start () {
		enableTransform = false;
		grab = null;
		grabbed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!laserLine.enabled && !shapeMenu.activeSelf && !soundMenu.activeSelf) {
			enableTransform = true;
		} else {
			enableTransform = false;
		}
		if (Controller.GetHairTriggerDown ()) {
			if (grab != null) {
				grabbed = true;
				linkObject (grab);
			}
		}
		if (Controller.GetHairTriggerUp ()) {
			if (grab != null) {
				grabbed = false;
				releaseObject (grab);
			}
		}

		// Delete objects by flicking controller up
		if (Mathf.Abs(Controller.velocity.y) > 1.5) {
			Vector3 vel = Controller.velocity;
			Debug.Log ("vel x = " + vel.x + "vel y = " + vel.y + "vel z = " + vel.z);
			if (grab != null && grabbed != true) {
				Destroy (grab);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log ("t-enter");
		if (enableTransform && (other.gameObject.layer == LayerMask.NameToLayer("instruments"))) {
			Debug.Log ("TRIGGER ENTER");
			grab = other.gameObject;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		grab = null;
	}

	private void linkObject(GameObject grabObject){
		grabObject.AddComponent<FixedJoint>();
		grabObject.GetComponent<FixedJoint>().connectedBody = trackedObj.GetComponent<Rigidbody>();
	}

	private void releaseObject(GameObject grabObject){
		if (grabObject.GetComponent<FixedJoint>() != null){
			Destroy (grabObject.GetComponent<FixedJoint> ());
		}
	}
}
