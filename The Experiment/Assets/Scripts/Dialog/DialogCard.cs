using UnityEngine;
using System.Collections;

public class DialogCard {
	public float textSpeed;
	public string dialog;
	public Color backgroundColor;

	/// <param name="textSpeed">Characters per second.</param>
	public DialogCard(float textSpeed, string dialog) {
		this.textSpeed = textSpeed;
		this.dialog = dialog;
		backgroundColor = Color.black;
		backgroundColor.a = 66;	//Set opacity at like 1/3
	}
}
