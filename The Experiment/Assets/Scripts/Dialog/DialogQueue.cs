using UnityEngine;
using System.Collections;
using System;

public class DialogQueue {
	private int dialogPointer = 0;
	private DialogCard[] queue;

	public DialogCard Next() {
		if (dialogPointer > queue.Length - 1)
			throw new OverflowException ("DialogQueue is empty.");
		DialogCard nextCard = queue [dialogPointer];
		dialogPointer += 1;
		return nextCard;
	}

	public bool HasNext() {
		return dialogPointer < queue.Length;
	}

	public void LoadQueue(DialogCard[] cards) {
		queue = cards;
		dialogPointer = 0;
	}

	public void ClearQueue() {
		queue = null;
		dialogPointer = 0;
	}

	public int Length() {
		return queue.Length;
	}
}
