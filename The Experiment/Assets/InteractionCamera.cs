using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// [RequireComponent(Camera)]

public class InteractionCamera : MonoBehaviour {
	private Dictionary<string, SmallInteractionObject> objectDictionary;

	private MeshFilter currentMeshFilter;
	private Mesh currentMesh;
	private DialogBox dialog;
	private Grayscale cameraGrayscale;
	private Camera camera;

	void Start () 
	{
		// These are probably going to be uniqe so grab them this way
		camera = GetComponent<Camera>();
		dialog = Object.FindObjectOfType<DialogBox>();
		cameraGrayscale = Object.FindObjectOfType<Grayscale>();
		objectDictionary = new Dictionary<string, SmallInteractionObject> ();
	}
	public void AddToDictionary (SmallInteractionObject newObject, Vector3 objectOrientation) {
		// Interaction objects MUST have a unique name!!!
		string key = newObject.name;
		if (objectDictionary.ContainsKey (key))
			Debug.LogError ("Object already exists in dictionary");
		else {
			objectDictionary.Add (key, newObject);
		}
	}
	public IEnumerator DisplayObject (string key) {
		SmallInteractionObject newDisplayObject = objectDictionary[key];
		currentMesh = newDisplayObject.mesh;
		currentMeshFilter = newDisplayObject.meshFilter;
		transform.localEulerAngles = newDisplayObject.interactionRotation;

		camera.enabled = true;
		cameraGrayscale.enabled = true;
		cameraGrayscale.effectAmount = 1;
		//dialog.SetDialogQueue(SplitCard(newDisplayObject.objectDialog));
		dialog.DisplayNextCard();

		while (dialog.IsDisplaying())
			yield return null;
		// Else...
		camera.enabled = false;
		cameraGrayscale.enabled = false;
		// Disables the object for future interactions if necessary.
		if (newDisplayObject.disableAfterUse)
			newDisplayObject.isUseable = false;
	}
}
