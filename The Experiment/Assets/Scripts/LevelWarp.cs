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
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(WarpTransition());
        }
    }

    public void Transition()
    {
        StartCoroutine(WarpTransition());
    }

    IEnumerator WarpTransition()
    {
        for (float t = 0, p = 0, time = 3f; t < time; t += Time.deltaTime, p = t / time)
        {
            grayscale.rampOffset = p;
            grayscale.effectAmount = p;
            yield return null;
        }

        bloom.enabled = true;
        motionBlur.enabled = true;

        if (!isInThePast)
        {
            player.transform.position = player.transform.position - difference;
            camera.transform.position = camera.transform.position - difference;
            pastEcho.enabled = true;
            pastDampening.enabled = true;
        }
        else
        {
            player.transform.position = player.transform.position + difference;
            camera.transform.position = camera.transform.position + difference;
            pastEcho.enabled = false;
            pastDampening.enabled = false;
        }

        isInThePast = !isInThePast;
        
        for (float t = 0, p = 0, time = 3f; t < time; t += Time.deltaTime, p = t / time)
        {
            grayscale.rampOffset = 1 - p;
            grayscale.effectAmount = 1 - p;
            yield return null;
        }
    }
}
