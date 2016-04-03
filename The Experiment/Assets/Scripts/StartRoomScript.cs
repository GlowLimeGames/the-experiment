using UnityEngine;
using System.Collections;

// Controls events in the starting room
public class StartRoomScript : MonoBehaviour 
{
    public DialogCard[] TolstoyDialog;
    public CameraTarget TolstoyFocus;

    public DialogCard[] TeaganDialog;
    public CameraTarget TeaganSpeachFocus;
    public CameraTarget TeaganControlFocus;
    public CameraTarget RatInteractionFocus;

    // Index int dialog at which we look over at Tolstoy
    public int tolstoyRevealIndex;

    public Animator cageAnimator;

    public Camera inspectionCamera;
    public DialogCard ratDialog;

    private CameraFollow cameraControl;
    private DialogBox dialog;
    private Grayscale cameraGrayscale;
    private KeyRat keyRat;
	
	void Start () 
    {
        // Since they're taking turns, enforce this
        if (TolstoyDialog.Length - TeaganDialog.Length > 1)
            Debug.LogError("Tolstoy dialog piece needs to be at most one longer than the other.");

        // These are probably going to be uniqe so grab them this way
        cameraControl = Object.FindObjectOfType<CameraFollow>();
        dialog = Object.FindObjectOfType<DialogBox>();
        cameraGrayscale = Object.FindObjectOfType<Grayscale>();
        keyRat = Object.FindObjectOfType<KeyRat>();

        StartCoroutine(StartRoomCoroutine());
	}

    IEnumerator StartRoomCoroutine()
    {
        inspectionCamera.enabled = false;
        cameraGrayscale.enabled = false;
        cameraControl.target = TeaganSpeachFocus;
        cameraControl.JumpToTarget();

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
                // Cheat - space to exit
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    dialog.StopAllCoroutines();
                    dialog.CleanDialogGUI();
                    cameraControl.target = TeaganControlFocus;
                    yield break;
                }
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
                    // Cheat - space to exit
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        dialog.StopAllCoroutines();
                        dialog.CleanDialogGUI();
                        cameraControl.target = TeaganControlFocus;
                        yield break;
                    }
                    yield return null;
                }
            }
        }

        cameraControl.target = RatInteractionFocus;

        while (!keyRat.Clicked)
            yield return null;

        inspectionCamera.enabled = true;
        cameraGrayscale.enabled = true;
        cameraGrayscale.effectAmount = 1;
        dialog.SetDialogQueue(SplitCard(ratDialog));
        dialog.DisplayNextCard();

        while (dialog.IsDisplaying())
            yield return null;

        inspectionCamera.enabled = false;
        cameraGrayscale.enabled = false;
        cageAnimator.SetBool("Open", true);

        cameraControl.target = TeaganControlFocus;
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
