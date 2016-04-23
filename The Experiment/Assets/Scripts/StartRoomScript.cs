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
    public GameObject inspectInstructions;
    public InteractionObject keyRat;
    public AudioSource cageDoorOpenSound;

    private CameraFollow cameraControl;
    private DialogBox dialog;
    private Grayscale grayscale;

    void Start()
    {
        // Since they're taking turns, enforce this
        if (TolstoyDialog.Length - TeaganDialog.Length > 1)
            Debug.LogError("Tolstoy dialog piece needs to be at most one longer than the other.");

        // These are probably going to be uniqe so grab them this way
        cameraControl = Object.FindObjectOfType<CameraFollow>();
        dialog = Object.FindObjectOfType<DialogBox>();
        grayscale = Object.FindObjectOfType<Grayscale>();

        StartCoroutine(StartRoomCoroutine());
    }

    IEnumerator StartRoomCoroutine()
    {
        cameraControl.target = TeaganSpeachFocus;
        cameraControl.JumpToTarget();

        dialogueInstructions.SetActive(false);
        mouseInstructions.SetActive(false);
        moveInstructions.SetActive(false);
        inspectInstructions.SetActive(false);

        float initialRampOffset = grayscale.rampOffset;
        grayscale.rampOffset = -1;
        grayscale.effectAmount = 1;

        yield return new WaitForSeconds(1);

        float time = 3f;
        for (float t = 0, p = 0f; t < time; t += Time.deltaTime, p = t / time)
        {
            grayscale.rampOffset = Mathf.Lerp(-1, initialRampOffset, p);
            grayscale.effectAmount = Mathf.Lerp(1, 0, p);
            yield return null;
        }

        dialogueInstructions.SetActive(true);

        int tolstoyIndex = 0; // TolstoyDialog.Length - 1;
        int teaganIndex = 0; // TeaganDialog.Length - 1;
        while (tolstoyIndex < TolstoyDialog.Length)
        {
            if (tolstoyIndex >= tolstoyRevealIndex)
                cameraControl.target = TolstoyFocus;

            dialog.SetDialogQueue(TolstoyDialog[tolstoyIndex]);
            dialog.DisplayNextCard();
            tolstoyIndex++;

            while (dialog.IsDisplaying())
            {
#if UNITY_EDITOR
                // Cheat - F to exit
                if (Input.GetKeyDown(KeyCode.F))
                {
                    dialog.StopAllCoroutines();
                    dialog.CleanDialogGUI();
                    cameraControl.target = TeaganControlFocus;
                    cageAnimator.SetBool("Open", true);
                    cageDoorOpenSound.Play();
                    yield break;
                }
#endif
                yield return null;
            }

            if (teaganIndex < TeaganDialog.Length)
            {
                cameraControl.target = TeaganSpeachFocus;
                dialog.SetDialogQueue(TeaganDialog[teaganIndex]);
                dialog.DisplayNextCard();
                teaganIndex++;

                while (dialog.IsDisplaying())
                {
#if UNITY_EDITOR
                    // Cheat - F to exit
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        dialog.StopAllCoroutines();
                        dialog.CleanDialogGUI();
                        cameraControl.target = TeaganControlFocus;
                        cageAnimator.SetBool("Open", true);
                        cageDoorOpenSound.Play();
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

        inspectInstructions.SetActive(true);
        mouseInstructions.SetActive(false);

        while (dialog.IsDisplaying())
            yield return null;

        cageAnimator.SetBool("Open", true);
        cageDoorOpenSound.Play();

        inspectInstructions.SetActive(false);
        moveInstructions.SetActive(true);

        cameraControl.target = TeaganControlFocus;

        yield return new WaitForSeconds(15f);

        moveInstructions.SetActive(false);
    }
}
