using UnityEngine;
using System.Collections;

public class LevelWarp : MonoBehaviour {
	Camera camera;
	GameObject player;
	bool isInThePast;

	public GameObject normalRoom;
	public GameObject pastRoom;
	// Audio filters should be placed on whatever has the audio listener (Main Camera)
	public AudioEchoFilter pastEcho;
	public AudioLowPassFilter pastDampening;
	Vector3 difference;

	void Start () {
		camera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		isInThePast = false;
		difference = normalRoom.transform.position - pastRoom.transform.position;

		if (camera == null)
			throw new UnassignedReferenceException ("No camera found.");
		if (player == null)
			throw new UnassignedReferenceException ("No player found.");
	}
	// For testing purposes
	void Update () {
		if (Input.GetKeyDown(KeyCode.L)) {
			StartCoroutine ("WarpTransition");
		}
	}

	public void RunBehavior () {
		StartCoroutine ("WarpTransition");
	}

	IEnumerator WarpTransition () {
		if (!isInThePast) {
			player.transform.position = player.transform.position - difference;
			camera.transform.position = camera.transform.position - difference;
			pastEcho.enabled = true;
			pastDampening.enabled = true;
		} else {
			player.transform.position = player.transform.position + difference;
			camera.transform.position = camera.transform.position + difference;
			pastEcho.enabled = false;
			pastDampening.enabled = false;
		}

		isInThePast = !isInThePast;
		yield return null;
	}
}
