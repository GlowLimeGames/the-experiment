using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Tooltip("Gameobject to follow.")]
    public GameObject target;
    
    [Tooltip("Determines how fast/slow the camera follow the player.")]
    [Range(0f, 1f)]
    public float followSpeed;

    [Tooltip("Use fixed update? (Important for syncing with physics)")]
    public bool useFixedUpdate;

    // The position the camera aims to be at if in room
    private Vector3 targetPosition;

    // How many vantage areas we are in
    private int vantageAreaCount = 0;

    [Tooltip("Spherical coordinates for the camera to start at.")]
    public float theta, phi, rho;

    [Tooltip("The how much we can zoom in and out.")]
    public float minZoom, maxZoom;

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
        Vector3 position;

        // Are we outside all vantage areas?
        if (vantageAreaCount == 0)
        {
            // If so, set the target position to follow the player
            position = GetTargetPosition();
        }
        else
        {
            position = targetPosition - (target.transform.position - targetPosition).normalized * rho;
        }

        // Set look direction towards the player
        transform.rotation = Quaternion.LookRotation(target.transform.position - this.transform.position);

        // Move towards target transform
        transform.position = Vector3.Lerp(transform.position, position, followSpeed);
    }

    void Update()
    {
        // Zoom in/out
        rho = Mathf.Clamp(rho + Input.mouseScrollDelta.y, minZoom, maxZoom);
        theta = Mathf.Lerp((Mathf.PI / 2f) * 7f / 8f, Mathf.PI / 8f, (rho - minZoom) / (maxZoom - minZoom));

        if (Input.GetKey(KeyCode.Q))
            phi -= Time.deltaTime;
        if (Input.GetKey(KeyCode.E))
            phi += Time.deltaTime;

        if (!useFixedUpdate) UpdatePosition();   
    }

    void FixedUpdate()
    {
        if (useFixedUpdate) UpdatePosition();   
    }

    Vector3 GetTargetPosition()
    {
        float x = rho * Mathf.Sin(theta) * Mathf.Cos(phi);
        float y = rho * Mathf.Cos(theta);
        float z = rho * Mathf.Sin(theta) * Mathf.Sin(phi);
        return new Vector3(x, y, z) + target.transform.position;
    }
}
