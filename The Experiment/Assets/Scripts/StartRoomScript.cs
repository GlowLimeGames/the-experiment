using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Controls events in the starting room
public class StartRoomScript : MonoBehaviour 
{
    public DialogCard[] TolstoyDialog;
    public CameraTarget TolstoyFocus;

    public DialogCard[] TeaganDialog;
    public CameraTarget TeaganSpeachFocus;
    public CameraTarget TeaganControlFocus;
    public CameraTarget RatInteractionFocus;

    // Index into dialog at which we look over at Tolstoy
    public int tolstoyRevealIndex;

    public Animator cageAnimator;

    public GameObject dialogueInstructions;
    public GameObject mouseInstructions;
    public GameObject moveInstructions;
    public InteractionObject keyRat;

    private CameraFollow cameraControl;
    private DialogBox dialog;
    
	void Start () 
    {
        // Since they're taking turns, enforce this
        if (TolstoyDialog.Length - TeaganDialog.Length > 1)
            Debug.LogError("Tolstoy dialog piece needs to be at most one longer than the other.");

        // These are probably going to be uniqe so grab them this way
        cameraControl = Object.FindObjectOfType<CameraFollow>();
        dialog = Object.FindObjectOfType<DialogBox>();

        StartCoroutine(StartRoomCoroutine());
	}

    IEnumerator StartRoomCoroutine()
    {
        cameraControl.target = TeaganSpeachFocus;
        cameraControl.JumpToTarget();

        dialogueInstructions.SetActive(true);
        mouseInstructions.SetActive(false);
        moveInstructions.SetActive(false);

        int tolstoyIndex = 0; // TolstoyDialog.Length - 1;
        int teaganIndex = 0; // TeaganDialog.Length - 1;
        while(tolstoyIndex < TolstoyDialog.Length)
        {
            if(tolstoyIndex >= tolstoyRevealIndex)
                cameraControl.target = TolstoyFocus;

            dialog.SetDialogQueue(SplitCard(TolstoyDialog[tolstoyIndex]));
            dialog.DisplayNextCard();
            tolstoyIndex++;

            while (dialog.IsDisplaying())
            {
#if UNITY_EDITOR
                // Cheat - space to exit
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    dialog.StopAllCoroutines();
                    dialog.CleanDialogGUI();
                    cameraControl.target = TeaganControlFocus;
                    yield break;
                }
#endif
                yield return null;
            }

            if (teaganIndex < TeaganDialog.Length)
            {
                cameraControl.target = TeaganSpeachFocus;
                dialog.SetDialogQueue(SplitCard(TeaganDialog[teaganIndex]));
                dialog.DisplayNextCard();
                teaganIndex++;

                while (dialog.IsDisplaying())
                {
#if UNITY_EDITOR
                    // Cheat - space to exit
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        dialog.StopAllCoroutines();
                        dialog.CleanDialogGUI();
                        cameraControl.target = TeaganControlFocus;
                        yield break;
                    }
#endif
                    yield return null;
                }
            }
        }

        dialogueInstructions.SetActive(false);
        mouseInstructions.SetActive(true);

        cameraControl.target = RatInteractionFocus;

        while (!keyRat.Clicked)
            yield return null;

        dialogueInstructions.SetActive(true);
        mouseInstructions.SetActive(false);
        
        while (dialog.IsDisplaying())
            yield return null;

        cageAnimator.SetBool("Open", true);

        dialogueInstructions.SetActive(false);
        moveInstructions.SetActive(true);

        cameraControl.target = TeaganControlFocus;

        yield return new WaitForSeconds(8f);

        moveInstructions.SetActive(false);
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
