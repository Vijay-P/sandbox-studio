using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSelectMenu : MonoBehaviour {


	private SteamVR_TrackedObject trackedObj;
	private GameObject selectedMenuItem; 

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	private int menu_status;
	private Color tmpColor;
	private string selectedShape;
	private string objectInHand; 

	void Start()
	{
		objectInHand = "";
		selectedShape = "";
	}
		

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	private void SetCollidingObject(Collider col)
	{
		if (selectedMenuItem || !col.GetComponent<Rigidbody>())
		{
			return;
		}
		selectedMenuItem = col.gameObject;
	}



	// On Trigger Methods
	public void OnTriggerEnter(Collider other)
	{
		SetCollidingObject(other);
		tmpColor = other.gameObject.GetComponentInParent<Renderer> ().material.color;
		other.gameObject.GetComponentInParent<Renderer> ().material.color = new Color (0, 255, 0);
		selectedShape = other.gameObject.GetComponentInChildren<TextMesh> ().text;
		Debug.Log ("Touching Shape = " + selectedShape);
	}

	public void OnTriggerStay(Collider other)
	{
		SetCollidingObject(other);
	}

	public void OnTriggerExit(Collider other)
	{

		other.gameObject.GetComponentInParent<Renderer> ().material.color = tmpColor;
		selectedShape = "";
	}

	// Select Menu Item and Add Joint
	private void SelectMenuItem()
	{
		//objectInHand = selectedMenuItem;
		//selectedMenuItem = null;

		//var joint = AddFixedJoint ();
		//joint.connectedBody = objectInHand.GetComponent<Rigidbody> ();
	}

	private FixedJoint AddFixedJoint()
	{
		FixedJoint fx = gameObject.AddComponent<FixedJoint> ();
		fx.breakForce = 20000;
		fx.breakTorque = 20000;
		return fx;
	}

	private void ReleaseObject(){
		if (GetComponent<FixedJoint>()){
			GetComponent<FixedJoint>().connectedBody = null;
			Destroy (GetComponent<FixedJoint> ());
		}
		objectInHand = "";
	}

	private GameObject GenerateShape(){
		GameObject shape = null;
		if (selectedShape == "Cube") {
			shape = GameObject.CreatePrimitive (PrimitiveType.Cube);
			shape.transform.position = trackedObj.transform.position;
			Vector3 scale_vec = new Vector3 (.1, .1, .1);
			shape.transform.localScale.Scale(scale_vec);
		} else if (selectedShape == "Sphere") {
			shape = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			shape.transform.position = trackedObj.transform.position;
		} else if (selectedShape == "Pyramid") {
			shape = GameObject.CreatePrimitive (PrimitiveType.Capsule);
			shape.transform.position = trackedObj.transform.position;
		} else {
			return shape;
		}
		shape.AddComponent<Rigidbody> ();
		return shape;
	}

	private void BindShape(GameObject shape){
		var joint = AddFixedJoint ();
		joint.connectedBody = shape.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Controller.GetHairTriggerDown ()) {
			if (selectedShape != "") {
				GameObject musical_shape = GenerateShape ();
				BindShape (musical_shape);
				objectInHand = selectedShape;
				selectedShape = "";
			}
		}

		if (Controller.GetHairTriggerUp ()) {
			if (objectInHand != "") {
				ReleaseObject ();
			}
		}
		
	}
}
