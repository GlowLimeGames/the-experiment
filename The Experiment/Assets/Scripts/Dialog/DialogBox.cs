using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(AudioSource))]
public class DialogBox : MonoBehaviour, IEventSystemHandler
{
    [Tooltip("xFaster the text displays at when F is held")]
    public float textSpeedUp = 2;
    public Color itemTextColor;

    private AudioSource audio;

    private DialogQueue queue;
    private DialogCard currentCard;
    private Image backgroundImage;
    private Text dialogText;
    private float currentTextSpeed;

    private float timeElapsed = 0f;
    private float charactersShowing = 0f;
    private bool allTextDisplayed = false;
    private bool colorTagOpen = false;
    private KeyCode advanceCode = KeyCode.Space;

    void Awake()
    {
        queue = new DialogQueue();
        backgroundImage = GetComponent<Image>();
        dialogText = GetComponentInChildren<Text>();
        audio = GetComponent<AudioSource>();

        backgroundImage.enabled = false;
        dialogText.enabled = false;
    }

    void Update()
    {
        // Sub with actual use key
        if (Input.GetKeyDown(advanceCode) && allTextDisplayed)
        {
            allTextDisplayed = false;
            DisplayNextCard();            
        }
    }

    // For singular cards
    public void SetDialogQueue(DialogCard card)
    {
        // Refactor later for better performance?
        if (queue == null) Debug.LogError("Why is this null?");

        queue.LoadQueue(SplitCard(card));
        currentCard = queue.Next();
    }

    public void SetDialogQueue(DialogCard[] cards)
    {
        // Refactor later for better performance?
        if (queue == null) Debug.LogError("Why is this null?");
        // Time for terrible hacky code.
        // For each card, we divide the card at its return characters, and add a list of all these expanded cards to the queue
        List<DialogCard> expandedCards = new List<DialogCard>();
        foreach (DialogCard card in cards)
        {
            expandedCards.AddRange(SplitCard(card));
        }
        queue.LoadQueue(expandedCards.ToArray());
        currentCard = queue.Next();
    }

    public void DisplayNextCard()
    {
        if (backgroundImage.enabled == false)
        {
            backgroundImage.enabled = true;
            dialogText.enabled = true;
        }
        if (currentCard != null)
        {
            StartCoroutine("TranscribeDialog", currentCard);
            if (queue.HasNext())
            {
                currentCard = queue.Next();
            }
            else
                currentCard = null;
        }
        else
        {
            CleanDialogGUI();
        }
    }

    public bool IsDisplaying()
    {
        return backgroundImage.enabled;
    }

    public void CleanDialogGUI()
    {
        audio.Stop();
        allTextDisplayed = false;
        colorTagOpen = false;
        backgroundImage.enabled = false;
        dialogText.text = "";
        dialogText.enabled = true;
        currentCard = null;
    }

    // Note that Color32 and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
    // (Thanks MVI & Unity wiki for this code!)
    string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
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

    IEnumerator TranscribeDialog(DialogCard card)
    {
        dialogText.color = card.textColor;
        audio.Play();

        while (charactersShowing <= card.dialog.Length - 1)
        {
            float speedMultiplier = 1f;
            int charactersInLastFrame = Mathf.FloorToInt(charactersShowing);
            string textToDisplay = "";

            charactersShowing += Time.deltaTime * card.textSpeed * speedMultiplier / 60f;

            int locationOfColorSymbol = card.dialog.Substring(charactersInLastFrame,
                Mathf.FloorToInt(charactersShowing) - charactersInLastFrame).IndexOf("*");

            // WARNING - could break if the machine somehow gets past both *s in one update
            // Also if keyword is last character
            if (locationOfColorSymbol != -1)
            {
                if (!colorTagOpen)
                {
                    // Tag length minus one for the * we are deleting
                    charactersShowing += 16f;
                    // Replace original card text with a rich-text coded string
                    card.dialog = card.dialog.Substring(0, charactersInLastFrame + locationOfColorSymbol) + "<color=#"
                        + ColorToHex(itemTextColor) + ">" + card.dialog.Substring(charactersInLastFrame
                            + locationOfColorSymbol + 1);
                    colorTagOpen = true;
                }
                else
                {
                    charactersShowing += 7f;
                    // Replace original card text with a rich-text coded string
                    card.dialog = card.dialog.Substring(0, charactersInLastFrame + locationOfColorSymbol) + "</color>"
                        + card.dialog.Substring(charactersInLastFrame + locationOfColorSymbol + 1);
                    colorTagOpen = false;
                }
            }
            if (colorTagOpen)
            {
                textToDisplay = card.dialog.Substring(0, Mathf.FloorToInt(charactersShowing)) + "</color>";
            }
            else
            {
                textToDisplay = dialogText.text = card.dialog.Substring(0, Mathf.FloorToInt(charactersShowing));
            }
            dialogText.text = textToDisplay;

            yield return null;
        }

        dialogText.text = card.dialog;
        allTextDisplayed = true;
        charactersShowing = 0;
        audio.Stop();
    }
}