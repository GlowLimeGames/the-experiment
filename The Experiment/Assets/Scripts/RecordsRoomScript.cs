using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RecordsRoomScript : MonoBehaviour
{
    public int numberOfPapers;
    public Conversation enterRoomConvo;
    public Conversation foundHalfRecordsConvo;
    public Conversation foundAllRecordsConvo;
    public DialogCard exitNotAllowed;
    public DialogCard exitAllowed;
    public AudioSource presentMusicSource;
    public AudioSource pastMusicSource;

    private ConversationManager conversationManager;
    private int numberOfPapersHalfway;

    void Start()
    {
        numberOfPapersHalfway = numberOfPapers / 2;
        conversationManager = FindObjectOfType<ConversationManager>();
        StartCoroutine(EnterRoomCoroutine());
    }

    private IEnumerator EnterRoomCoroutine()
    {
        var grayscale = FindObjectOfType<Grayscale>();
        yield return grayscale.Fade(false, 3f);

        // Let the player wander for a bit, then play the conversation
        yield return new WaitForSeconds(5f);

        var conversationManager = FindObjectOfType<ConversationManager>();
        conversationManager.RunConversation(enterRoomConvo);
    }
    
    public void ReadPaper()
    {
        numberOfPapers--;

        if (numberOfPapers == numberOfPapersHalfway)
            conversationManager.RunConversation(foundHalfRecordsConvo);

        if (numberOfPapers == 0)
            conversationManager.RunConversation(foundAllRecordsConvo);
    }

    public void AttemptExitStart(InteractionObject obj)
    {
        obj.objectDialog = numberOfPapers == 0 ? exitAllowed : exitNotAllowed;
    }

    public void AttemptExitEnd(InteractionObject obj)
    {
        if (numberOfPapers == 0)
        {
            var closeSound = obj.GetComponent<AudioSource>();
            if (closeSound != null)
                closeSound.Play();

            StartCoroutine(ExitCoroutine());
        }
    }

    private IEnumerator ExitCoroutine()
    {
        yield return FindObjectOfType<Grayscale>().Fade(true, 3f);

        // End of playable game, go to main menu
        SceneManager.LoadScene("MainMenu");
    }
}
