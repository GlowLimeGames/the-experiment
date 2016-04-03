using UnityEngine;
using System.Collections;

public class SmallInteractionObject : MonoBehaviour 
{
	//public Renderer outlineRenderer;
	public Vector3 interactionRotation;
	public Vector3 interactionScale;
	public DialogCard objectDialog;
	public MeshRenderer outlineRenderer;

	public bool isUseable = true;
	public bool disableAfterUse = true;

	public GameObject interactionObject { get; private set;}
	InteractionCameraView interactionCameraView;
	GameObject player;

	public bool Clicked { get; private set; }

	void Start()
	{
		outlineRenderer.enabled = false;

		interactionScale = transform.localScale;
		interactionObject = gameObject;
		interactionCameraView = FindObjectOfType<InteractionCameraView> ();

		player = GameObject.FindGameObjectWithTag ("Player");


		if (player == null)
			Debug.LogError ("Player not found; set its tag to Player");

		interactionCameraView.AddToDictionary (this);
	}

	void OnMouseOver()
	{
		if (isUseable && outlineRenderer != null)
			outlineRenderer.enabled = true;
	}

	void OnMouseExit()
	{
		if (isUseable && outlineRenderer != null)
			outlineRenderer.enabled = false;
	}

	void OnMouseDown()
	{
		if (isUseable && !interactionCameraView.IsDisplaying()) {
			print ("Mouse click registered");
			// Add stopped movement
			interactionCameraView.DisplayObject (name, player.transform.localEulerAngles);
			Clicked = true;
			if (disableAfterUse)
				isUseable = false;
		}
	}
}