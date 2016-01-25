using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Tooltip("Gameobject to follow.")]
    public GameObject target;

    [Tooltip("Distance away from target camera will stay when following.")]
    public Vector3 followingOffset;

    [Tooltip("Determines how fast/slow the camera follow the player.")]
    [Range(0f, 1f)]
    public float followSpeed;

    [Tooltip("Use fixed update? (Important for syncing with physics)")]
    public bool useFixedUpdate;

    // The position the camera aims to be at
    private Vector3 targetPosition;

    // How many vantage areas we are in
    private int vantageAreaCount = 0;

    void Start()
    {
        // Listen to when the vantage areas are entered/exited
        foreach (var vantage in GameObject.FindObjectsOfType<CameraVantage>())
        {
            vantage.OnVantageAreaEntered += VantageAreaEntered;
            vantage.OnVantageAreaExited += VantageAreaExited;
        }
    }

    void VantageAreaEntered(CameraVantage vantage, Collider other)
    {
        // This assumes the collider used by the player is on the same gameobject we're following which may have to change
        if (other.gameObject == target)
        {
            vantageAreaCount++;
            targetPosition = vantage.cameraPosition.position;
        }
    }

    void VantageAreaExited(CameraVantage vantage, Collider other)
    {
        // Same here ^
        if (other.gameObject == target)
        {
            vantageAreaCount--;
        }
    }

    void UpdatePosition()
    {
        // Are we outside all vantage areas?
        if (vantageAreaCount == 0)
        {
            // If so, set the target position to follow the player
            targetPosition = target.transform.position + followingOffset;
        }

        // Set look direction towards the player
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - this.transform.position);

        // Move towards target transform
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, followSpeed);
    }

    void Update()
    {
        // Zoom in/out
        followingOffset = followingOffset.normalized * Mathf.Clamp(followingOffset.magnitude + Input.mouseScrollDelta.y, 1f, 10f);

        if (!useFixedUpdate) UpdatePosition();   
    }

    void FixedUpdate()
    {
        if (useFixedUpdate) UpdatePosition();   
    }
}
