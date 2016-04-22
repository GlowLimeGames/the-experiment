using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RecordsRoomOpen : MonoBehaviour
{
    public Conversation enterConversation;

    public void EnterRecordsRoom()
    {
        StartCoroutine(EnterRecordsRoomCoroutine());
    }

    private IEnumerator EnterRecordsRoomCoroutine()
    {
        var conversationManager = FindObjectOfType<ConversationManager>();
        var grayscale = FindObjectOfType<Grayscale>();

        yield return conversationManager.RunConversation(enterConversation);
        yield return grayscale.Fade(true, 3f);
        SceneManager.LoadScene("RecordRoom");
    }
}
