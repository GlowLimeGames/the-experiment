using UnityEngine;
using System.Collections;

public class ConversationManager : MonoBehaviour 
{
    public GameObject teaganRoot;
    public GameObject tolstoyRoot;

    public CameraTarget teaganFollowTarget;

    // Prefab
    public CameraTarget conversationTargetPrefab;
    private CameraTarget conversationTarget;

    public bool IsInProgress { private set; get; }

    private CameraFollow camera;
    private DialogBox dialog;

	void Start () 
    {
        conversationTarget = Instantiate<GameObject>(conversationTargetPrefab.gameObject).GetComponent<CameraTarget>();
        camera = FindObjectOfType<CameraFollow>();
        dialog = FindObjectOfType<DialogBox>();   
	}

    public Coroutine RunConversation(Conversation conversation)
    {
        return StartCoroutine(RunConversationCoroutine(conversation));
    }

    private IEnumerator RunConversationCoroutine(Conversation conversation)
    {
        IsInProgress = true;

        Coroutine alignCoroutine = conversation.alignCharacters ? StartCoroutine(AlignCharactersCoroutine()) : null;

        if (conversation.controlCamera)
            camera.target = conversationTarget;
        
        foreach(DialogCard card in conversation.Cards())
        {
            dialog.SetDialogQueue(card);
            dialog.DisplayNextCard();
            while (dialog.IsDisplaying())
                yield return null;

            yield return null;
        }

        if (alignCoroutine != null)
            StopCoroutine(alignCoroutine);
        camera.target = teaganFollowTarget;

        IsInProgress = false;
    }

    private IEnumerator AlignCharactersCoroutine()
    {
        while (true)
        {
            Vector3 center = (teaganRoot.transform.position + tolstoyRoot.transform.position) / 2;

            // Place the camera between Teagan and Tolstoy, but not more than 2 units away from Teagan
            Ray r = new Ray(teaganRoot.transform.position, center - teaganRoot.transform.position);
            float distance = Mathf.Min(2f, (center - teaganRoot.transform.position).magnitude);
            conversationTarget.transform.position = r.GetPoint(distance);
            conversationTarget.transform.rotation = Quaternion.LookRotation(Vector3.Cross(center - teaganRoot.transform.position, Vector3.up));

            center.y = teaganRoot.transform.position.y;
            teaganRoot.transform.rotation = Quaternion.Lerp(teaganRoot.transform.rotation, Quaternion.LookRotation(center - teaganRoot.transform.position), 0.1f);

            center.y = tolstoyRoot.transform.position.y;
            tolstoyRoot.transform.rotation = Quaternion.Lerp(tolstoyRoot.transform.rotation, Quaternion.LookRotation(center - tolstoyRoot.transform.position), 0.1f);

            yield return null;
        }
    }
}
