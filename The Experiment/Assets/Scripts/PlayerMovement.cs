using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float moveSpeed = 5f;
	public float turnSpeed = 180f;

	private float currentRotation = 0f;
	private bool rotating = false;
	private Rigidbody rb;
	private Animator anim;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		anim = GetComponentInChildren<Animator> ();
	}

	public void Move(float horizontal, float vertical){
		if (horizontal == 0 && vertical == 0) {
			Stop (); // if not moving, stop rotating
			anim.SetBool("Moving", false);
		} else {
			anim.SetBool ("Moving", true);
		}

		if (horizontal < 0 && vertical > 0) {
			StartCoroutine ("RotatePlayer", new Vector3(0, 315)); // Northwest
		}
		else if (horizontal < 0 && vertical < 0) {
			StartCoroutine ("RotatePlayer", new Vector3(0, 225)); // Southwest
		}
		else if (horizontal > 0 && vertical > 0) {
			StartCoroutine ("RotatePlayer", new Vector3(0, 45)); // Northeast
		}
		else if (horizontal > 0 && vertical < 0) {
			StartCoroutine ("RotatePlayer", new Vector3(0, 135)); // SouthEast
		}
		else if (horizontal > 0 && vertical == 0) {
			StartCoroutine ("RotatePlayer", new Vector3(0, 90)); // East
		}
		else if (horizontal < 0 && vertical == 0) {
			StartCoroutine ("RotatePlayer", new Vector3(0, 270)); // West
		}
		else if (vertical > 0 && horizontal == 0) {
			StartCoroutine ("RotatePlayer", new Vector3(0, 0)); // North
		}
		else if (vertical < 0 && horizontal == 0) {
			StartCoroutine ("RotatePlayer", new Vector3(0, 180)); // South
		}


		rb.MovePosition (transform.position + transform.forward * moveSpeed * Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical)) * Time.deltaTime);

	}
	void Stop(){
		rotating = false;
		StopCoroutine("RotatePlayer");
		currentRotation = transform.localEulerAngles.y;
	}

	IEnumerator RotatePlayer(Vector3 endRotation){
		if (currentRotation != endRotation.y) {
			if (!rotating) {
				rotating = true;
				float amountToRotate = Mathf.Abs (currentRotation - endRotation.y);
				int dir = 1;
				if (currentRotation > endRotation.y) {
					dir = -1;
				} else {
					dir = 1;
				}
				if (amountToRotate >= 180f) { //if greater than 180 find the quicker way around
					amountToRotate = 360f - amountToRotate; 
					dir = -dir;
				}

				if (amountToRotate == 180f) {
					if (endRotation.y == 90f) {
						dir = -dir;
					}
				}
				float rotationDoneSoFar = 0.0f;

				while (rotationDoneSoFar <= amountToRotate) {
					float rotateBy = Time.deltaTime * turnSpeed;
					transform.Rotate (Vector3.up, rotateBy * dir);
					rotationDoneSoFar += rotateBy;

					yield return null;
				}
				transform.rotation = Quaternion.Euler (endRotation); // for cleaner angles
				currentRotation = endRotation.y;
				rotating = false;
			}
		} else {
			//Do nothing
		}
	}
}
