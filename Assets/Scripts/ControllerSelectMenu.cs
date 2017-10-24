using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSelectMenu : MonoBehaviour {

	public Material activeMaterial;

	private SteamVR_TrackedObject trackedObj;
	private GameObject selectedMenuItem; 
	private Material tmpColor;
	private string selectedShape;
	private GameObject objectInHand; 
	private LineRenderer laserLine;

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}
		
	void Awake()
	{
		laserLine = GetComponent<LineRenderer> ();
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}


	void Start()
	{
		objectInHand = null;
		selectedShape = "";
	}

	// Update is called once per frame
	void Update () {

		if (!laserLine.enabled) {
			if (Controller.GetHairTriggerDown ()) {
				if (selectedShape != "") {
					GenerateAndBindShape ();
					selectedShape = "";
				}
			}

			if (Controller.GetHairTriggerUp ()) {
				if (objectInHand != null) {
					ReleaseObject ();
				}
			}
		}

	}

	private void GenerateAndBindShape(){
		switch(selectedShape){
		case("Cube"):
			objectInHand = initShape(PrimitiveType.Cube);
			break;
		case("Sphere"):
			objectInHand = initShape(PrimitiveType.Sphere);
			break;
		case("Pyramid"):
			objectInHand = initShape(PrimitiveType.Capsule);
			break;	
		}
		objectInHand.AddComponent<FixedJoint>();
		objectInHand.GetComponent<FixedJoint>().connectedBody = trackedObj.GetComponent<Rigidbody>();
	}

	private GameObject initShape(PrimitiveType shapeType){
		objectInHand = GameObject.CreatePrimitive (shapeType);
		objectInHand.layer = LayerMask.NameToLayer ("instruments");
		objectInHand.transform.position = trackedObj.transform.position + trackedObj.transform.forward * 0.5f;
		objectInHand.AddComponent<Rigidbody> ();
		objectInHand.GetComponent<Rigidbody> ().useGravity = false;
		objectInHand.GetComponent<Rigidbody> ().isKinematic = false;
		objectInHand.GetComponent<Collider> ().isTrigger = true;
		objectInHand.AddComponent<PlaySound> ();
		objectInHand.GetComponent<PlaySound> ().active = activeMaterial;
		return objectInHand;
	}

	private void ReleaseObject(){
		if (objectInHand.GetComponent<FixedJoint>() != null){
			objectInHand.GetComponent<FixedJoint>().connectedBody = null;
			Destroy (objectInHand.GetComponent<FixedJoint> ());
			objectInHand = null;
		}
	}

	// On Trigger Methods
	private void OnTriggerEnter(Collider other)
	{
		SetCollidingObject(other);
		Renderer menu = other.gameObject.GetComponentInParent<Renderer> ();
		if (other.gameObject.layer == LayerMask.NameToLayer("menu_item")) {
			tmpColor = menu.material;
			menu.material = activeMaterial;
			TextMesh shapeComponent = other.gameObject.GetComponentInChildren<TextMesh> ();
			if (shapeComponent != null) {
				selectedShape = shapeComponent.text;
				Debug.Log ("Touching Shape = " + selectedShape);
			}
		}
	}

	private void SetCollidingObject(Collider col){
		if (selectedMenuItem || !col.GetComponent<Rigidbody>()){
			return;
		}
		selectedMenuItem = col.gameObject;
	}
		

	private void OnTriggerStay(Collider other){
		SetCollidingObject(other);
	}

	private void OnTriggerExit(Collider other){
		if (other.gameObject.layer == LayerMask.NameToLayer ("menu_item")) {
			other.gameObject.GetComponentInParent<Renderer> ().material = tmpColor;
			selectedShape = "";
		}
	}
		
}