using UnityEngine;
using UnityEngine.UI;

public class Socket : ControlNode
	{
	MovableNode subNode;
		
	public void SetSubnode(MovableNode node) {
		Reset ();
		subNode = node;
		subNode.SetPosition (transform.position);
	}

	public MovableNode GetSubnode() {
		return subNode;
	}

	public override void Activate () {
		if (subNode != null) {
			subNode.Activate ();
		} else {
			base.Activate ();
		}
	}

	public override void Reset ()
	{
		if (subNode != null)
			subNode.Reset();
	}

	public void ClearSubnode() {
		subNode = null;
	}

	public bool IsEmpty() {
		return (subNode == null);
	}
}

