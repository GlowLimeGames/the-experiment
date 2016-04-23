using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class LevelWarp : MonoBehaviour
{
    public GameObject normalRoom;
    public GameObject pastRoom;
    public Conversation jumpToPastConvo;
    public AudioSource presentMusicSource;
    public AudioSource pastMusicSource;

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
        pastMusicSource.volume = 0;

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
            StartCoroutine(LerpAudioSourceVolume(presentMusicSource, 0, 3f));
            StartCoroutine(LerpAudioSourceVolume(pastMusicSource, 1, 3f));

            player.transform.position = player.transform.position - difference;
            camera.transform.position = camera.transform.position - difference;
            bloom.enabled = true;
            motionBlur.enabled = true;
        }
        else
        {
            StartCoroutine(LerpAudioSourceVolume(presentMusicSource, 0.5f, 3f));
            StartCoroutine(LerpAudioSourceVolume(pastMusicSource, 0, 3f));

            player.transform.position = player.transform.position + difference;
            camera.transform.position = camera.transform.position + difference;
            bloom.enabled = false;
            motionBlur.enabled = false;
        }

        isInThePast = !isInThePast;

        yield return grayscale.Fade(false, 3f, false);
    }

    private IEnumerator LerpAudioSourceVolume(AudioSource source, float endVolume, float time)
    {
        float startVolume = source.volume;
        for (float t = 0, p = 0; t < time; t += Time.deltaTime, p = t / time)
        {
            source.volume = Mathf.Lerp(startVolume, endVolume, p);
            yield return null;
        }
    }
}
