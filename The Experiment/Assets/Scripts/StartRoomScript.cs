using UnityEngine;
using System.Collections;

// Controls events in the starting room
public class StartRoomScript : MonoBehaviour 
{
    public DialogCard[] TolstoyIntroSpeech;
    public float grayscaleFadeTime;

    public CameraTarget TolstoyFocus;
    public CameraTarget TeaganFocus;

    private CameraFollow cameraControl;
    private DialogBox dialog;
	
	void Start () 
    {
        // These are probably going to be uniqe so grab them this way
        cameraControl = Object.FindObjectOfType<CameraFollow>();
        dialog = Object.FindObjectOfType<DialogBox>();
        StartCoroutine(StartRoomCoroutine());
	}

    IEnumerator StartRoomCoroutine()
    {
        cameraControl.target = TolstoyFocus;

        dialog.SetDialogQueue(TolstoyIntroSpeech);
        dialog.DisplayNextCard();

        while (dialog.IsDisplaying())
            yield return null;

        cameraControl.target = TeaganFocus;
    }
}
