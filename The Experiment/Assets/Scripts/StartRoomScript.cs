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

    public Animator cageAnimator;

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
        int tolstoyIndex = 0;
        int teaganIndex = 0;
        while(tolstoyIndex < TolstoyDialog.Length)
        {
            cameraControl.target = TolstoyFocus;
            dialog.SetDialogQueue(new DialogCard[] { TolstoyDialog[tolstoyIndex] });
            dialog.DisplayNextCard();
            tolstoyIndex++;

            while (dialog.IsDisplaying())
                yield return null;

            if (teaganIndex < TeaganDialog.Length)
            {
                cameraControl.target = TeaganSpeachFocus;
                dialog.SetDialogQueue(new DialogCard[] { TeaganDialog[teaganIndex] });
                dialog.DisplayNextCard();
                teaganIndex++;

                while (dialog.IsDisplaying())
                    yield return null;
            }
        }

        cageAnimator.SetBool("Open", true);
        cameraControl.target = TeaganControlFocus;
    }
}
