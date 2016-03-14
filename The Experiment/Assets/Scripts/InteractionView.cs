using UnityEngine;
using System.Collections;

// Controls events in the starting room
public class InteractionView : MonoBehaviour 
{
	public Animator cageAnimator;

	public bool disableOnClose;
	public Camera inspectionCamera;
	public DialogCard objectDialog;

	private DialogBox dialog;
	private Grayscale cameraGrayscale;

	void Start () 
	{
		// These are probably going to be uniqe so grab them this way
		dialog = Object.FindObjectOfType<DialogBox>();
		cameraGrayscale = Object.FindObjectOfType<Grayscale>();

		//StartCoroutine(InteractionCoroutine(null));
	}

	IEnumerator InteractionCoroutine(GameObject interactionObject)
	{
		inspectionCamera.enabled = true;
		cameraGrayscale.enabled = true;
		cameraGrayscale.effectAmount = 1;
		dialog.SetDialogQueue(SplitCard(objectDialog));
		dialog.DisplayNextCard();

		while (dialog.IsDisplaying())
			yield return null;

		inspectionCamera.enabled = false;
		cameraGrayscale.enabled = false;

		// Run actions associated with interaction completion
		interactionObject.SetActive(false);
		// cageAnimator.SetBool ("Open", true);
	}

	private DialogCard[] SplitCard(DialogCard card)
	{
		string[] parts = card.dialog.Split('\n');
		DialogCard[] cards = new DialogCard[parts.Length];

		for (int i = 0; i < parts.Length; i++)
		{
			cards[i] = new DialogCard(card.textSpeed, parts[i]);
			cards[i].textColor = card.textColor;
			cards[i].backgroundColor = card.backgroundColor;
		}

		return cards;
	}
}
