using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSelectMenu : MonoBehaviour {

	public Material activeMaterial;
	private SteamVR_TrackedObject trackedObj;
	private GameObject selectedMenuItem; 

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	private int menu_status;
	private Material tmpColor;
	private string selectedShape;
	private GameObject objectInHand; 

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}


	void Start()
	{
		objectInHand = null;
		selectedShape = "";
	}

	// Update is called once per frame
	void Update () {
		if (Controller.GetHairTriggerDown ()) {
			if (selectedShape != "") {
				GenerateAndBindShape ();
				selectedShape = "";
			}
		}

		if (Controller.GetHairTriggerUp ()) {
			ReleaseObject ();
		}

	}

	void GenerateAndBindShape(){
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
		objectInHand.transform.position = trackedObj.transform.position + trackedObj.transform.forward * 1.0f;
		objectInHand.AddComponent<Rigidbody> ();
		objectInHand.GetComponent<Rigidbody> ().useGravity = false;
		objectInHand.GetComponent<Rigidbody> ().isKinematic = false;
		return objectInHand;
	}

	private void ReleaseObject(){
		if (objectInHand.GetComponent<FixedJoint>() != null){
			Debug.Log ("joint does not exist");
			objectInHand.GetComponent<FixedJoint>().connectedBody = null;
			Destroy (objectInHand.GetComponent<FixedJoint> ());
			objectInHand.transform.parent = null;
			objectInHand = null;
		}
		Debug.Log ("Inside Rel o");
	}

	// On Trigger Methods
	public void OnTriggerEnter(Collider other)
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
		

	public void OnTriggerStay(Collider other){
		SetCollidingObject(other);
	}

	public void OnTriggerExit(Collider other){
		if (other.gameObject.layer == LayerMask.NameToLayer ("menu_item")) {
			other.gameObject.GetComponentInParent<Renderer> ().material = tmpColor;
			selectedShape = "";
		}
	}
		
}