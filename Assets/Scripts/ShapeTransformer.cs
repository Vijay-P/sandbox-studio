using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeTransformer : MonoBehaviour {

	public GameObject shapeMenu;
	public GameObject soundMenu;
	private SteamVR_TrackedObject trackedObj;
	private LineRenderer laserLine;
	private bool enableTransform;
	public bool act;
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
				linkObject (grab);
			}
		}
		if (Controller.GetHairTriggerUp ()) {
			if (grab != null) {
				releaseObject (grab);
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
