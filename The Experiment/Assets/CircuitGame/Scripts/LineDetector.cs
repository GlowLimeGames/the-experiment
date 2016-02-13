using UnityEngine;
using System.Collections;

public class LineDetector : MonoBehaviour 
{
	public bool isConnected = false;
	public bool isBox = false;
	public bool isCross = false;

	public GameObject connectedTo;
	private SpriteRenderer sprite;

	// Use this for initialization
	void Start () 
	{
		sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (connectedTo != null) {
			if (connectedTo.GetComponent<LineDetector> ().isConnected == false) {
				isConnected = false;
				connectedTo = null;
			}
		}
		if (isCross) {
			if (isConnected) {
				Transform[] children = new Transform[transform.childCount];
				for (int i = 0; i < transform.childCount; i++) {
					children [i] = transform.GetChild (i);
				}
				foreach (Transform child in children){
					SpriteRenderer sprite = child.GetComponent<SpriteRenderer>();
					sprite.color = new Color (255, 255, 255, 1);
				}
			}else{
				Transform[] children = new Transform[transform.childCount];
				for (int i = 0; i < transform.childCount; i++) {
					children [i] = transform.GetChild (i);
				}
				foreach (Transform child in children){
					SpriteRenderer sprite = child.GetComponent<SpriteRenderer>();
					sprite.color = new Color (255, 255, 255, .3f);
				}
			}
		}
		else {
			if (isConnected) {
				sprite.color = new Color (255, 255, 255, 1);
			} else {
				sprite.color = new Color (255, 255, 255, .3f);
			}
		}
	}

	void OnTriggerStay2D (Collider2D other)
	{
		if (connectedTo == null) {
			if (other.gameObject.GetComponent<LineDetector> () != null) {
				if (other.gameObject.GetComponent<LineDetector> ().isConnected == true) {
					if (other.gameObject.GetComponent<LineDetector> ().connectedTo != this.gameObject) {
						isConnected = true;
						connectedTo = other.gameObject;
					}
				}
			}
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (!isBox) {
			if (isConnected) {
				connectedTo = null;
				isConnected = false;
			}
		}
	}
}
