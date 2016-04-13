using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HallwayOpen : MonoBehaviour
{
    public DialogCard keycardNotFoundDialog;
    public DialogCard keycardFoundDialog;

    private bool keycardFound = false;
    private Grayscale grayscale;

    public void FoundKeycard(InteractionObject obj)
    {
        keycardFound = true;
        grayscale = GameObject.FindObjectOfType<Grayscale>();
    }

    public void GoToHallwayBegin(InteractionObject obj)
    {
        // Modify the dialog to indicate whether the key card has been found or not.
        obj.objectDialog = keycardFound ? keycardFoundDialog : keycardNotFoundDialog;
    }

    // Only load hallway if keycard has been found
    public void GoToHallwayEnd(InteractionObject obj)
    {
        if(keycardFound)
        {
            StartCoroutine(ExitToHallwayCoroutine());
            
        }
    }

    IEnumerator ExitToHallwayCoroutine()
    {
        float initialRampOffset = grayscale.rampOffset;
        float time = 3f;

        for (float t = 0, p = 0; t < time; t += Time.deltaTime, p = t / time)
        {
            grayscale.rampOffset = Mathf.Lerp(initialRampOffset, -1, p);
            grayscale.effectAmount = Mathf.Lerp(0, 1, p);
            yield return null;
        }

        // Go to main menu for demo
        SceneManager.LoadScene("MainMenu");
    }
}
