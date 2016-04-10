using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HallwayOpen : MonoBehaviour
{
    public DialogCard keycardNotFoundDialog;
    public DialogCard keycardFoundDialog;

    bool keycardFound = false;

    public void FoundKeycard(InteractionObject obj)
    {
        keycardFound = true;
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
            SceneManager.LoadScene("FirstHallway");
        }
    }
}
