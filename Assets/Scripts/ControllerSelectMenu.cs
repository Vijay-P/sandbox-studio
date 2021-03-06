﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSelectMenu : MonoBehaviour {

	public Material activeMaterial;
	public Material inactiveMaterial;
	public GameObject shapeMenu;
	public GameObject soundMenu;
	public GameObject orb;

	public GameObject natureSubMenu;
	public Material natureSoundMaterial;

	private SteamVR_TrackedObject trackedObj;
	private GameObject selectedMenuItem; 
	private string selectedShape;
	private GameObject selectedSoundType;
	private GameObject selectedShapeType;
	private string selectedSoundNote;
	private GameObject objectInHand; 
	private LineRenderer laserLine;
	private GameObject submenu;
	private GameObject selectedSound;
	private AudioClip selection;
	private GameObject applyShape;
	private GameObject activeSoundType;
	private Material soundMaterial;

	//private 

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
		selectedSoundNote = "";
		selectedSoundType = null;
		submenu = null;
		selectedSound = null;
		selection = null;
		orb.SetActive (false);
		applyShape = null;
		activeSoundType = null;
		soundMaterial = null;
		selectedMenuItem = null;
		selectedShapeType = null;
	}

	// Update is called once per frame
	void Update () {

		if (!laserLine.enabled) {

			//Shape Menu Interaction
			if (shapeMenu.activeSelf) {
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

			// Sound Menu Interaction
			else if (soundMenu.activeSelf) {
				if (Controller.GetHairTriggerDown ()) {
					Debug.Log ("Choose or Bind");
					ChooseOrBindSound ();
				}
				if (Controller.GetHairTriggerUp () && orb.activeSelf) {
					Debug.Log ("Apply Sound");
					ApplySound ();
				}
			}
		}

	}

	private void ApplySound(){
		if (applyShape != null && selection != null) {
			PlaySound myshape = applyShape.GetComponent<PlaySound> ();
			myshape.hitSound = selection;
			myshape.GetComponent<Renderer> ().material = soundMaterial;
			myshape.inactive = soundMaterial;
		}
		applyShape = null;
		selection = null;
		selectedSound = null;
		orb.SetActive (false);
	}

	// Determine whether to pop up sub-menu or bind sound
	private void ChooseOrBindSound(){
		if (selectedSound != null) {
			Debug.Log("Pick Sound");
			PickSound ();
		}else if (selectedSoundType != null) {
			Debug.Log ("Expand Menu");
			ExpandSoundMenu ();
		}
	}

	private void PickSound(){
		SoundContainer container = selectedSound.GetComponent<SoundContainer> ();
		soundMaterial = container.material;
		if (container != null) {
			selection = container.sound;
			orb.SetActive (true);
		}
	}

	// Expand Submenu
	private void ExpandSoundMenu(){
		if (activeSoundType != null) {
			activeSoundType.GetComponent<Renderer> ().material = inactiveMaterial;
		}
		if (submenu != null) {
			Destroy (submenu);
			submenu = null;
			soundMaterial = null;
		}

		// build new submenu (if selectedSoundType != activeSoundType)
		if (!selectedSoundType.Equals (activeSoundType)) {
			activeSoundType = selectedSoundType;
			selectedSoundType.GetComponent<Renderer> ().material = activeMaterial;

			TextMesh sound_type = selectedSoundType.transform.GetChild (0).GetComponent<TextMesh>();
			Debug.Log ("Sound Selected = " + sound_type.text);

			if (sound_type.text.Equals("Nature")) {
				soundMaterial = natureSoundMaterial;
				submenu = Instantiate (natureSubMenu, soundMenu.transform.position + soundMenu.transform.right * .35f, soundMenu.transform.rotation, soundMenu.transform) as GameObject;
			} else {
				ObjectContainer container = selectedSoundType.GetComponent<ObjectContainer> ();
				if (container != null) {
					soundMaterial = container.soundMaterial;
					submenu = Instantiate (container.child_object, soundMenu.transform.position + soundMenu.transform.right * .35f, soundMenu.transform.rotation, soundMenu.transform) as GameObject;
				}
			}
		} else {
			activeSoundType = null;
		}
			
//		if (selectedSoundType != null) {
//			if (submenu == null) {
//				tmpColor = selectedSoundType.GetComponent<Renderer> ().material;
//				selectedSoundType.GetComponent<Renderer> ().material = activeMaterial;
//				ObjectContainer container = selectedSoundType.GetComponent<ObjectContainer> ();
//				if (container != null) {
//					submenu = Instantiate (container.child_object, soundMenu.transform.position + soundMenu.transform.right * .35f, soundMenu.transform.rotation, soundMenu.transform) as GameObject;
//				}
//			} else {
//				selectedSoundType.GetComponent<Renderer> ().material = tmpColor;
//				Destroy (submenu);
//				submenu = null;
//			}
//
//		}
		
	}

	// Generate the new shape and bind to controller
	private void GenerateAndBindShape(){
		Debug.Log (selectedShape);
		switch(selectedShape){
		case("Cube"):
			objectInHand = GameObject.CreatePrimitive (PrimitiveType.Cube);
			initShape ();
			objectInHand.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			break;
		case("Sphere"):
			objectInHand = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			initShape();
			objectInHand.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			break;
		case("Pyramid"):
			ObjectContainer container = selectedShapeType.GetComponent<ObjectContainer> ();
			objectInHand = Instantiate (container.child_object) as GameObject;
			initShape ();
			objectInHand.transform.localScale = new Vector3 (30.0f, 30.0f, 30.0f);
			break;	
		}
		objectInHand.AddComponent<FixedJoint>();
		objectInHand.GetComponent<FixedJoint>().connectedBody = trackedObj.GetComponent<Rigidbody>();
	}

	// initialize the shape
	private void initShape(){
		objectInHand.layer = LayerMask.NameToLayer ("instruments");
		objectInHand.transform.position = trackedObj.transform.position + trackedObj.transform.forward * 0.5f;
		objectInHand.AddComponent<Rigidbody> ();
		objectInHand.GetComponent<Rigidbody> ().useGravity = false;
		objectInHand.GetComponent<Rigidbody> ().isKinematic = false;
		objectInHand.GetComponent<Collider> ().isTrigger = true;
		objectInHand.AddComponent<PlaySound> ();
		objectInHand.GetComponent<PlaySound> ().active = activeMaterial;
	}

	// Release the object
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
		if (other.gameObject.layer == LayerMask.NameToLayer ("menu_item_shape")) {
			selectedShapeType = other.gameObject;
			menu.material = activeMaterial;
			TextMesh shapeComponent = other.gameObject.GetComponentInChildren<TextMesh> ();
			if (shapeComponent != null) {
				selectedShape = shapeComponent.text;
				Debug.Log ("Touching Shape = " + selectedShape);
			}
		} else if (other.gameObject.layer == LayerMask.NameToLayer ("menu_item_soundType")) {
			menu.material = activeMaterial;
			selectedSoundType = other.gameObject;
		} else if (other.gameObject.layer == LayerMask.NameToLayer ("menu_item_sound")) {
			menu.material = activeMaterial;
			selectedSound = other.gameObject;
		} else if (orb.activeSelf) {
			applyShape = other.gameObject;
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
		if (other.gameObject.layer == LayerMask.NameToLayer ("menu_item_shape")) {
			other.gameObject.GetComponentInParent<Renderer> ().material = inactiveMaterial;
			selectedShape = "";
			selectedShapeType = null;
		} else if (other.gameObject.layer == LayerMask.NameToLayer ("menu_item_soundType")) {
			other.gameObject.GetComponentInParent<Renderer> ().material = inactiveMaterial;
			selectedSoundType = null;
		} else if (other.gameObject.layer == LayerMask.NameToLayer ("menu_item_sound")) {
			other.gameObject.GetComponentInParent<Renderer> ().material = inactiveMaterial;
			selectedSound = null;
		} else if (orb.activeSelf) {
			applyShape = null;
		}
	}
		
}