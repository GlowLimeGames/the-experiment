using UnityEngine;
using System.Collections;

public class ConversationManager : MonoBehaviour 
{
    public GameObject teaganRoot;
    public GameObject tolstoyRoot;

    public CameraTarget teaganFollowTarget;
    public CameraTarget teaganTarget;
    public CameraTarget tolstoyTarget;

    private CameraFollow camera;
    private DialogBox dialog;

	void Start () 
    {
        camera = FindObjectOfType<CameraFollow>();
        dialog = FindObjectOfType<DialogBox>();   
	}

    public Coroutine RunConversation(Conversation conversation)
    {
        return StartCoroutine(RunConversationCoroutine(conversation));
    }

    private IEnumerator RunConversationCoroutine(Conversation conversation)
    {
        Coroutine alignCoroutine = StartCoroutine(AlignCharactersCoroutine());

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

            yield return null;
        }

        StopCoroutine(alignCoroutine);
        camera.target = teaganFollowTarget;
    }

    private IEnumerator AlignCharactersCoroutine()
    {
        while (true)
        {
            Vector3 center = (teaganRoot.transform.position + tolstoyRoot.transform.position) / 2;

            center.y = teaganRoot.transform.position.y;
            teaganRoot.transform.rotation = Quaternion.Lerp(teaganRoot.transform.rotation, Quaternion.LookRotation(center - teaganRoot.transform.position), 0.1f);

            center.y = tolstoyRoot.transform.position.y;
            tolstoyRoot.transform.rotation = Quaternion.Lerp(tolstoyRoot.transform.rotation, Quaternion.LookRotation(center - tolstoyRoot.transform.position), 0.1f);

            yield return null;
        }
    }
}
