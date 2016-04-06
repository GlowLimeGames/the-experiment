using UnityEngine;
using System.Collections;

public class PlayerMovementWASD : MonoBehaviour {

	public float moveSpeed = 5f;
	public float turnSpeed = 180f;

	private Rigidbody rb;
	private Animator anim;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		anim = GetComponentInChildren<Animator> ();
	}

	public void Move(float horizontal, float vertical){
		if (horizontal == 0 && vertical == 0) {
			anim.SetBool("Moving", false);
		} else {
			anim.SetBool ("Moving", true);
		}

		transform.Rotate (Vector3.up * horizontal * turnSpeed);

        if (vertical < 0) vertical *= 0.2f;
		rb.MovePosition (transform.position + transform.forward * moveSpeed * vertical * Time.deltaTime);
	}

	public void StopMovement() {
		anim.SetBool("Moving", false);
	}
}
