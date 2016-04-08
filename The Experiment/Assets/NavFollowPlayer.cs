using UnityEngine;
using System.Collections;

public class NavFollowPlayer : MonoBehaviour {

	public GameObject target;
	public bool follow = false;

	private Animator anim;
	private NavMeshAgent nav;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		nav = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (follow) {
			Vector3 targetDir = target.transform.position - transform.position;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 1.5f * Time.deltaTime, 0.0F);
			transform.rotation = Quaternion.LookRotation(newDir);

			nav.SetDestination (target.transform.position);
			anim.SetBool ("Walking", true);
		} else {
			anim.SetBool ("Walking", false);
		}

		if (follow) {
			nav.Resume ();
		} else {
			nav.Stop ();
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			follow = false;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			follow = true;
		}
	}
}
