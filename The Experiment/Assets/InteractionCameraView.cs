using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// [RequireComponent(Camera)]

public class InteractionCameraView : MonoBehaviour {
	public Dictionary<string, SmallInteractionObject> objectDictionary;

	private MeshFilter currentMeshFilter;
	private MeshRenderer currentMeshRenderer;
	private DialogBox dialog;
	public Grayscale cameraGrayscale;
	public Camera camera;

	void Start () 
	{
		// These are probably going to be unique so grab them this way
		//camera = GetComponent<Camera>();
		dialog = Object.FindObjectOfType<DialogBox>();
		cameraGrayscale = Object.FindObjectOfType<Grayscale>();

		currentMeshFilter = GetComponent<MeshFilter> ();
		currentMeshRenderer = GetComponent<MeshRenderer> ();

		objectDictionary = new Dictionary<string, SmallInteractionObject> ();
		camera.enabled = false;
	}
	public void AddToDictionary (SmallInteractionObject newObject) {
		// Interaction objects MUST have a unique name!!!
		print("Object added to dictionary");
		string key = newObject.name;
		if (objectDictionary.ContainsKey (key))
			Debug.LogError ("Object already exists in dictionary");
		else {
			objectDictionary.Add (key, newObject);
		}
	}
	public IEnumerator DisplayObject (string key) {
		print ("Activated");
		SmallInteractionObject newDisplayObject = objectDictionary[key];
		currentMeshRenderer = newDisplayObject.meshRenderer;
		currentMeshFilter.mesh = newDisplayObject.meshFilter.mesh;
		transform.localEulerAngles = newDisplayObject.interactionRotation;
		transform.localScale = newDisplayObject.interactionScale;

		camera.enabled = true;
		cameraGrayscale.enabled = true;
		cameraGrayscale.effectAmount = 1;
		dialog.SetDialogQueue(newDisplayObject.objectDialog);
		dialog.DisplayNextCard();

		while (dialog.IsDisplaying())
			yield return null;
		// Else...
		camera.enabled = false;
		cameraGrayscale.enabled = false;
		// Disables the object for future interactions if necessary.
		if (!newDisplayObject.disableAfterUse)
			newDisplayObject.isUseable = true;
		//yield return null;
	}
}
