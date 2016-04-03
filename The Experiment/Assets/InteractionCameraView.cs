using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// [RequireComponent(Camera)]

public class InteractionCameraView : MonoBehaviour {
	public Dictionary<string, SmallInteractionObject> objectDictionary;

	private GameObject viewObject;
	private DialogBox dialog;
	private Vector3 defaultPosition;
	private Vector3 viewBounds;
	Grayscale cameraGrayscale;
	Camera camera;

	public GameObject[] messageTarget;
	public string message;

	void Start () 
	{
		// These are probably going to be unique so grab them this way
		viewObject = GameObject.Find("ViewObject");
		if (viewObject == null)
			Debug.LogError ("ViewObject not found!");
		
		dialog = Object.FindObjectOfType<DialogBox>();
		cameraGrayscale = Object.FindObjectOfType<Grayscale>();
		camera = GameObject.FindGameObjectWithTag ("InteractionCamera").GetComponent<Camera>();
		if (camera.transform.rotation != Quaternion.Euler(Vector3.zero))
			Debug.LogError ("The scale to fit function is gonna bug out unless you change the rotation of the camera to zero.");

		defaultPosition = viewObject.transform.position;

		objectDictionary = new Dictionary<string, SmallInteractionObject> ();
		camera.enabled = false;
	}
	public void AddToDictionary (SmallInteractionObject newObject) {
		// Interaction objects MUST have a unique name!!!
		string key = newObject.name;

		if (objectDictionary.ContainsKey (key))
			Debug.LogError ("Object already exists in dictionary");
		else {
			objectDictionary.Add (key, newObject);
		}
	}

	public bool IsDisplaying () {
		return camera.enabled;
	}

	public void DisplayObject (string key, Vector3 playerEulers) {

		SmallInteractionObject newDisplayObject = objectDictionary [key];
		Quaternion objectRotation = Quaternion.Euler (newDisplayObject.interactionRotation);

		Destroy (viewObject);
		newDisplayObject.isUseable = false;

		viewObject = (GameObject)Instantiate(newDisplayObject.interactionObject, 
			defaultPosition, 
			objectRotation);

		//viewObject.layer = gameObject.layer;
		// ScaleObjectToFit();
		print("Frame height: " + (2.0f * (transform.position.z) * Mathf.Tan (camera.fieldOfView * 0.5f * Mathf.Deg2Rad)));
		print ("Frame width: " + (2.0f * (transform.position.z) * Mathf.Tan (camera.fieldOfView * 0.5f * Mathf.Deg2Rad)) / camera.aspect);
		print ("Lossy Y: " + viewObject.transform.lossyScale.y);
		print ("Lossy X: " + viewObject.transform.lossyScale.x);


		camera.enabled = true;
		cameraGrayscale.enabled = true;
		cameraGrayscale.effectAmount = 1;
		if (newDisplayObject.objectDialog != null)
			dialog.SetDialogQueue (newDisplayObject.objectDialog);
		dialog.DisplayNextCard ();

		StartCoroutine (CloseInteractionCamera ());
	}

	void ScaleObjectToFit() {
		float largestSide;
		float scaleFactor;
		// float frustumBound;
		float lossyY = viewObject.transform.lossyScale.y;
		float lossyX = viewObject.transform.lossyScale.x;
		// Caution: this code is incredibly hacky.
		float frustumHeight = 2.0f * (transform.position.z) * Mathf.Tan (camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
		float frustumWidth = frustumHeight * camera.aspect;

		if (lossyX > lossyY) {
			largestSide = lossyX;
			scaleFactor = frustumWidth / lossyX;
		} else {
			largestSide = lossyY;
			scaleFactor = frustumHeight / lossyY;
		}
		Vector3 newScale = viewObject.transform.localScale;
		for (int i = 0; i < 3; i++) {
			newScale [i] = newScale[i] * scaleFactor;
			}
		viewObject.transform.localScale = newScale;

	}
		IEnumerator CloseInteractionCamera () {
		while (dialog.IsDisplaying())
			yield return null;
		// Else...
		camera.enabled = false;
		cameraGrayscale.enabled = false;
		// Run world behaviors resulting from interaction
		if (messageTarget.Length > 0) {
			foreach (GameObject target in messageTarget) {
				target.SendMessage ("RunBehavior()");
			}
		}
	}
}