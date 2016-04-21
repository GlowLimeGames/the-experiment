using UnityEngine;
using System.Collections;

public class ConversationManager : MonoBehaviour 
{
    public CameraTarget teaganTarget;
    public CameraTarget tolstoyTarget;

    private CameraFollow camera;
    private DialogBox dialog;

	void Start () 
    {
        camera = FindObjectOfType<CameraFollow>();
        dialog = FindObjectOfType<DialogBox>();   
	}

    public void RunConversation(Converstation conversation)
    {
        StartCoroutine(RunConversationCoroutine(conversation));
    }

    private IEnumerator RunConversationCoroutine(Converstation conversation)
    {
        foreach(DialogCard card in conversation.Cards())
        {
            if (card is OwnedDialogCard)
            {
                if((card as OwnedDialogCard).isTeagan)
                    camera.target = teaganTarget;
                else
                    camera.target = tolstoyTarget;
            }

            dialog.SetDialogQueue(card);
            dialog.DisplayNextCard();
            while (dialog.IsDisplaying())
                yield return null;
        }
    }
}
