using UnityEngine;
using System.Collections;

public class RecordsRoomScript : MonoBehaviour
{
    public Conversation enterRoomConvo;

    void Start()
    {
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
}
