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
        cameraControl.target = TolstoyFocus;
        cameraControl.JumpToTarget();

        int tolstoyIndex = TolstoyDialog.Length - 1;
        int teaganIndex = TeaganDialog.Length - 1;
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

        cameraControl.target = TeaganControlFocus;

        while (!keyRat.Clicked)
            yield return null;

        inspectionCamera.enabled = true;
        cameraGrayscale.enabled = true;
        cameraGrayscale.effectAmount = 1;
        dialog.SetDialogQueue(new DialogCard[] { ratDialog });
        dialog.DisplayNextCard();

        while (dialog.IsDisplaying())
            yield return null;

        inspectionCamera.enabled = false;
        cameraGrayscale.enabled = false;
        cageAnimator.SetBool("Open", true);
    }
}
