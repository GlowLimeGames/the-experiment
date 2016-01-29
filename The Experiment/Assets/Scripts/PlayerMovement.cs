using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float moveSpeed = 5f;
	public float m_StationaryTurnSpeed = 180f;
	public float m_MovingTurnSpeed = 360f;
	private Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	public void Move(float horizontal, float vertical){

		float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, horizontal); //turn faster when moving
		transform.Rotate(Vector3.up, horizontal * turnSpeed * Time.deltaTime); // if horizontal input rotate

		float moveDir = moveSpeed * vertical; // if vertical input move position
		rb.MovePosition (transform.position + transform.forward * moveDir * Time.deltaTime);

	}
}
