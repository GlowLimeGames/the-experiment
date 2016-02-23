using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FlowPuzzleController : MonoBehaviour
	{
	public List<ControlNode> nodeList;
	public bool isRunning = false;

	public void startRunning() {
		StartCoroutine("RunFlow");
	}

	public void stopRunning() {
		// Not working
		isRunning = false;
	}

	IEnumerator RunFlow() {
		isRunning = true;
		print ("Started");
		while (isRunning) {
			ControlNode[] nodes = nodeList.ToArray();
			print (nodes.Length);
			nodeList.Clear ();

				foreach (ControlNode node in nodes) {
				node.Activate ();
				ControlNode[] nextNodes = node.GetNext ();

				for (int i = 0; i < nextNodes.Length; i++) {
					if (!nodeList.Contains(nextNodes[i])) {
						nodeList.Add(nextNodes[i]);
						}
					}
				}
			yield return new WaitForSeconds (0.5f);
		}
		// Clear all nodes when no longer running
	}
}

