using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class ControlNode : MonoBehaviour
{
	public ControlNode[] nextNodes;
	protected Image icon;
	protected Color defaultColor;
	protected RectTransform transform;

	public virtual void Start () {
		icon = GetComponent<Image> ();
		transform = GetComponent<RectTransform> ();
		defaultColor = icon.color;
	}

	// Returns the node which is next in exection?
	public virtual void Activate() {
		StartCoroutine ("colorChange");
	}

	public virtual void Reset () {
		icon.color = defaultColor;
		return;
	}

	public ControlNode[] GetNext(){
		return nextNodes;
	}

	public void SetNext(ControlNode[] nodes) {
		nextNodes = nodes;
	}

	protected IEnumerator colorChange() {
		icon.color = Color.black;
		yield return new WaitForSeconds (0.5f);
		icon.color = defaultColor;
		yield return null;
	}

}

