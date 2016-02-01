using UnityEngine;
using System.Collections;

public class TeganAnimationController : MonoBehaviour {
	private Animator teganAnimator;
	private Transform teganTransform;
	private Vector3 lastPosition;
	// Use this for initialization
	void Start () {
		teganAnimator = GetComponent<Animator> ();
		teganTransform = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 currentPosition = teganTransform.position;
		if (currentPosition != lastPosition){
		Vector3 differenceOfPosition = lastPosition - currentPosition;

			if (differenceOfPosition.x > 0) {
				if (differenceOfPosition.z > 0) {
					teganTransform.localEulerAngles = new Vector3 (0, -135);
				} else if (differenceOfPosition.z < 0) {
					teganTransform.localEulerAngles = new Vector3 (0, -45);
				} else {
					teganTransform.localEulerAngles = new Vector3 (0, -90);
				}
			} else if (differenceOfPosition.x < 0) {
				if (differenceOfPosition.z > 0) {
					teganTransform.localEulerAngles = new Vector3 (0, 135);
				} else if (differenceOfPosition.z < 0) {
					teganTransform.localEulerAngles = new Vector3 (0, 45);
				} else {
					teganTransform.localEulerAngles = new Vector3 (0, 90);
				}
			} else if (differenceOfPosition.z > 0) {
				teganTransform.localEulerAngles = new Vector3 (0, 180);
			} else if (differenceOfPosition.z < 0) {
				teganTransform.localEulerAngles = new Vector3 (0, 0);
			}
				lastPosition = currentPosition;
		}
	}

}
