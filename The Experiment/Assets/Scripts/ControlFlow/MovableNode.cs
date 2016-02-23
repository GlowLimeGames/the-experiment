using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(BoxCollider2D))]
public class MovableNode : ControlNode
	{
	private bool isBeingDragged = false;
	private Socket potentialSocket;
	//private 

	/*public override void Activate() {
		int butts = 1;
		return;
	}*/

	public void Reset() {
		isBeingDragged = false;
		potentialSocket = null;
	}
		
	public void PickedUp() {
		isBeingDragged = true;
		if (potentialSocket != null) {
			if (potentialSocket.GetSubnode() == this) {
				potentialSocket.ClearSubnode ();
			}
		}
		StartCoroutine ("UpdatePosition");
	}

	public void PutDown() {
		isBeingDragged = false;
	}

	public void SetPosition(Vector2 newPosition) {
		transform.position = newPosition;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		print("Trigger Entered");
		if (other.gameObject.GetComponent<Socket> () != null) {
			potentialSocket = other.gameObject.GetComponent<Socket> ();
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.GetComponent<Socket> () != null) {
			potentialSocket = null;
		}
	}

	IEnumerator UpdatePosition() {
		float delta = 0;

		while (isBeingDragged) {
			// The commented out code allows us to introduce a lag to the movement of the nodes, consistent with the computer analogy
			/*delta += Time.deltaTime;
			if (delta >= 0.02f) {
				delta = 0;*/
				transform.position = Input.mousePosition;
			//}
			yield return null;
		}
		if (potentialSocket != null && potentialSocket.IsEmpty()) {
						potentialSocket.SetSubnode(this);
		}
	}

}

