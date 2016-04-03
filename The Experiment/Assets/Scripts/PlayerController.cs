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

	void FixedUpdate() 
    {
		// read inputs
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

        if (dialogBox == null || !dialogBox.IsDisplaying())
        {
            p_Movement.Move(h, v);
        }
        else
        {
            p_Movement.Move(0, 0);
        }
	}
}
