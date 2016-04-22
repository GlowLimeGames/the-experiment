using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class LevelWarp : MonoBehaviour
{
    public GameObject normalRoom;
    public GameObject pastRoom;

    // Audio filters should be placed on whatever has the audio listener (Main Camera)
    public AudioEchoFilter pastEcho;
    public AudioLowPassFilter pastDampening;

    public Conversation jumpToPastConvo;

    private Vector3 difference;
    private Camera camera;
    private GameObject player;
    private bool isInThePast;
    private MotionBlur motionBlur;
    private BloomOptimized bloom;
    private Grayscale grayscale;

    void Start()
    {
        camera = GetComponent<Camera>();
        bloom = GetComponent<BloomOptimized>();
        motionBlur = GetComponent<MotionBlur>();
        grayscale = GetComponent<Grayscale>();

        bloom.enabled = false;
        motionBlur.enabled = false;

        isInThePast = false;
        difference = normalRoom.transform.position - pastRoom.transform.position;

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            throw new UnassignedReferenceException("No player found.");
    }

    // For testing purposes
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(WarpTransition());
        }
#endif
    }

    public void TransitionToPast()
    {
        StartCoroutine(TransitionToPastCoroutine());
    }

    private IEnumerator TransitionToPastCoroutine()
    {
        yield return Transition();
        yield return new WaitForSeconds(2f);
        yield return FindObjectOfType<ConversationManager>().RunConversation(jumpToPastConvo);
    }

    public Coroutine Transition()
    {
        return StartCoroutine(WarpTransition());
    }

    IEnumerator WarpTransition()
    {
        yield return grayscale.Fade(true, 3f, false);

        if (!isInThePast)
        {
            player.transform.position = player.transform.position - difference;
            camera.transform.position = camera.transform.position - difference;
            pastEcho.enabled = true;
            pastDampening.enabled = true;
            bloom.enabled = true;
            motionBlur.enabled = true;
        }
        else
        {
            player.transform.position = player.transform.position + difference;
            camera.transform.position = camera.transform.position + difference;
            pastEcho.enabled = false;
            pastDampening.enabled = false;
            bloom.enabled = false;
            motionBlur.enabled = false;
        }

        isInThePast = !isInThePast;

        yield return grayscale.Fade(false, 3f, false);
    }
}
