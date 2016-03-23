using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float grabRange = 5.0f;
	public float grabRadius = 5.0f;
	public DialogBox dialogBox;

	private bool inRangeInteract = false;
	private PlayerMovementWASD p_Movement;
	// Use this for initialization
	void Start () {
		p_Movement = GetComponent<PlayerMovementWASD> ();
		dialogBox = FindObjectOfType<DialogBox> ();
	}

	void FixedUpdate() {
			// read inputs
			float h = Input.GetAxis ("Horizontal");
			float v = Input.GetAxis ("Vertical");

			p_Movement.Move (h, v);

			// Raycast from player to see what is in front of player
			bool interact = Input.GetKeyDown (KeyCode.F);

			Ray grabRay = new Ray (transform.position, transform.forward);
			RaycastHit hit;
			if (Physics.SphereCast (grabRay, grabRadius, out hit, grabRange)) {
				if (hit.transform.CompareTag ("Usable")) {
					inRangeInteract = true;
					if (interact) {
						//p_Movement.StopMovement ();
						// Player rotation is sent to viewable object to orient it correctly
						//hit.transform.gameObject.SendMessage ("Use", transform.localEulerAngles, SendMessageOptions.DontRequireReceiver);
						// Items with usable tag will have a Use function
					}
				}
			} else {
				inRangeInteract = false;
			}

			// if in range with interactable object display interact button
			if (CanvasController.Instance) {
				if (inRangeInteract) {
					CanvasController.Instance.DisplayInteractButton (true);
				} else {
					CanvasController.Instance.DisplayInteractButton (false);
			}
		}
	}
}
