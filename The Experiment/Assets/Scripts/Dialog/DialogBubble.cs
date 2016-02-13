using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DialogBubble : MonoBehaviour, IEventSystemHandler {
	private DialogQueue queue;
	private DialogCard currentCard;
	private Image backgroundImage;
	private Text dialogText;
	private float currentTextSpeed;

	private float timeElapsed = 0f;
	private bool allTextDisplayed = false;

	void Start () {
		queue = new DialogQueue ();
		backgroundImage = GetComponent<Image> ();
		dialogText = GetComponentInChildren<Text> ();

		backgroundImage.enabled = false;
		dialogText.enabled = false;
	}

	void Update	() {
		// Sub with actual use key
		if (Input.GetKeyDown(KeyCode.F) && allTextDisplayed) {
			allTextDisplayed = false;
			DisplayNextCard ();
		}
	}

	public void SetDialogQueue(DialogCard[] cards) {
		if (cards.Length == 1)
			currentCard = cards [0];
		else {
			// Refactor later for better performance?
			queue.LoadQueue (cards);
			currentCard = queue.Next();
		}
	}

	public void DisplayNextCard() {
		print ("Display Next called");
		if (backgroundImage.enabled == false) {
			backgroundImage.enabled = true;
			dialogText.enabled = true;
		}
		if (currentCard != null) {
			StartCoroutine ("TranscribeDialog", currentCard);
			if (queue.HasNext ()) {
				currentCard = queue.Next ();
				print (currentCard.dialog);
			}
			else
				currentCard = null;
		} else {
			CleanDialogGUI ();
		}
	}

	void CleanDialogGUI() {
		backgroundImage.enabled = false;
		dialogText.text = "";
		dialogText.enabled = true;
		currentCard = null;
	}

	IEnumerator TranscribeDialog(DialogCard card) {
		print ("Hello");
		float charactersToDisplay = 0f;

		while (charactersToDisplay <= card.dialog.Length - 1) {
			charactersToDisplay = timeElapsed * card.textSpeed / 60f;
			dialogText.text = card.dialog.Substring (0, Mathf.FloorToInt (charactersToDisplay));
			timeElapsed += Time.deltaTime;
			yield return null;
		}
		dialogText.text = card.dialog;
		allTextDisplayed = true;
		print (allTextDisplayed);
		timeElapsed = 0;
	}

}
