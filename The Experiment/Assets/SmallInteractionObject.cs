using UnityEngine;
using System.Collections;

public class SmallInteractionObject : MonoBehaviour 
{
	//public Renderer outlineRenderer;
	public Vector3 interactionRotation;
	public Vector3 interactionScale;
	public DialogCard objectDialog;
	public MeshRenderer meshRenderer;
	public MeshFilter meshFilter;

	public bool isUseable = true;
	public bool disableAfterUse = true;

	public InteractionCameraView interactionCameraObject;

	public bool Clicked { get; private set; }

	void Start()
	{
		//outlineRenderer.enabled = false;
		meshRenderer = GetComponent<MeshRenderer> ();
		meshFilter = GetComponent<MeshFilter> ();
		interactionScale = transform.localScale;
		interactionRotation = transform.localEulerAngles;
		interactionCameraObject.AddToDictionary (this);
	}

	/*void OnMouseOver()
	{
		if (isUseable)
			outlineRenderer.enabled = true;
	}

	void OnMouseExit()
	{
		if (isUseable)
			outlineRenderer.enabled = false;
	}*/

	void OnMouseDown()
	{
		if (isUseable) {
			interactionCameraObject.DisplayObject (name);
			Clicked = true;
		}
	}

	public void Use () {
		if (isUseable) {
			isUseable = false;
			StartCoroutine (interactionCameraObject.DisplayObject (name));
		}
	}
}