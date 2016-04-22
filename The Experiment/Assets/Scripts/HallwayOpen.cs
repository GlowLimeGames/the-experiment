using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HallwayOpen : MonoBehaviour
{
    public Conversation keycardNotFoundConvo;
    public Conversation keycardFoundConvo;

    private bool keycardFound = false;
    private Grayscale grayscale;
    private ConversationManager convoManager;

    void Start()
    {
        grayscale = FindObjectOfType<Grayscale>();
        convoManager = FindObjectOfType<ConversationManager>();
    }

    public void FoundKeycard(InteractionObject obj)
    {
        keycardFound = true;
    }

    public void GoToHallwayBegin(InteractionObject obj)
    {
        // Make sure no dialog runs
        obj.objectDialog = null;
    }
        
    public void GoToHallwayEnd(InteractionObject obj)
    {
        // Only load hallway if keycard has been found
        if (keycardFound)
        {
            StartCoroutine(ExitToHallwayCoroutine());
        }
        else
        {
            convoManager.RunConversation(keycardNotFoundConvo);
        }
    }    

    IEnumerator ExitToHallwayCoroutine()
    {
        yield return convoManager.RunConversation(keycardFoundConvo);

        // Fade out
        yield return grayscale.Fade(true, 3f);

        // Go to main menu for demo
        SceneManager.LoadScene("FirstHallway");
    }
}
