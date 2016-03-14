using UnityEngine;
using System.Collections;

public class SmallInteractionObject : MonoBehaviour 
{
	public Renderer outlineRenderer;
	public Vector3 interactionRotation;
	public DialogCard objectDialog;
	public Mesh mesh;
	public MeshFilter meshFilter;

	public bool isUseable = true;
	public bool disableAfterUse = true;

	public InteractionCamera interactionCamera;

	public bool Clicked { get; private set; }

	void Start()
	{
		outlineRenderer.enabled = false;
		mesh = GetComponent<Mesh> ();
		meshFilter = GetComponent<MeshFilter> ();
		interactionCamera.AddToDictionary (this, interactionRotation);
	}

	void OnMouseOver()
	{
		if (isUseable)
			outlineRenderer.enabled = true;
	}

	void OnMouseExit()
	{
		if (isUseable)
			outlineRenderer.enabled = false;
	}

	void OnMouseDown()
	{
		if (isUseable) {
			interactionCamera.DisplayObject (name);
			Clicked = true;
		}
	}
}