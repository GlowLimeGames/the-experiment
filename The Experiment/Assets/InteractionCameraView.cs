using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// [RequireComponent(Camera)]

public class InteractionCameraView : MonoBehaviour {
	public Dictionary<string, SmallInteractionObject> objectDictionary;

	private GameObject viewObject;
	private DialogBox dialog;
	private Vector3 defaultPosition;
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

		defaultPosition = viewObject.transform.localPosition;

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

		Destroy (viewObject);

		viewObject = (GameObject)Instantiate(newDisplayObject.interactionObject, 
			defaultPosition, 
			Quaternion.Euler(newDisplayObject.interactionObject.transform.localEulerAngles - playerEulers));
		
		camera.enabled = true;
		cameraGrayscale.enabled = true;
		cameraGrayscale.effectAmount = 1;
		if (newDisplayObject.objectDialog != null)
			dialog.SetDialogQueue (newDisplayObject.objectDialog);
		dialog.DisplayNextCard ();

		StartCoroutine (CloseInteractionCamera ());
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