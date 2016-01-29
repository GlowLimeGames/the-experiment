using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float grabRange = 5.0f;

	private bool inRangeInteract = false;
	private PlayerMovement p_Movement;
	// Use this for initialization
	void Start () {
		p_Movement = GetComponent<PlayerMovement> ();

	}

	void FixedUpdate() {
		// read inputs
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		p_Movement.Move (h, v);

		// Raycast from player to see what is in front of player
		bool interact = Input.GetKey (KeyCode.F);

		Ray grabRay = new Ray (transform.position + Vector3.up * 0.5f, transform.forward);
		RaycastHit hit;
		if (Physics.Raycast (grabRay,out hit, grabRange)){
			if (hit.transform.CompareTag ("Usable")) {
				inRangeInteract = true;
				if (interact) {
					hit.transform.gameObject.SendMessage ("Use", SendMessageOptions.DontRequireReceiver); 
					// Items with usable tag will have a Use function
				}
			}
		}else {
			inRangeInteract = false;
		}

		// if in range with interactable object display interact button
		if (inRangeInteract) {
			CanvasController.Instance.DisplayInteractButton (true);
		} else {
			CanvasController.Instance.DisplayInteractButton (false);
		}
	}
}
