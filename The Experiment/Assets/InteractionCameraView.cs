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

		currentMeshRenderer.materials = new Material[4];
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

	public void DisplayObject (string key, Vector3 playerEulers, Material[] materials) {
		currentMeshFilter.mesh.Clear ();
		currentMeshRenderer.materials = new Material[materials.Length];

		SmallInteractionObject newDisplayObject = objectDictionary [key];
		currentMeshFilter.mesh = newDisplayObject.mesh;
		currentMeshRenderer = newDisplayObject.meshRenderer;
		currentMeshRenderer.materials = materials;

		transform.localEulerAngles = newDisplayObject.interactionRotation - playerEulers;
		transform.localScale = newDisplayObject.interactionScale;

		camera.enabled = true;
		cameraGrayscale.enabled = true;
		cameraGrayscale.effectAmount = 1;
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
	}
}
