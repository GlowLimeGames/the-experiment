using UnityEngine;
using System;

[RequireComponent(typeof(BoxCollider))]
public class CameraVantage : MonoBehaviour
{
    [Tooltip("The position the camera will be at this vantage point.")]
    public Transform cameraPosition;

    // This event is triggered in OnTriggerEnter
    public event Action<CameraVantage, Collider> OnVantageAreaEntered;

    // This event is triggered in OnTriggerExit
    public event Action<CameraVantage, Collider> OnVantageAreaExited;

    void Start()
    {
        if (!this.GetComponent<BoxCollider>().isTrigger)
        {
            Debug.LogWarning("The collider should be set to trigger.");
            this.GetComponent<BoxCollider>().isTrigger = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (OnVantageAreaEntered != null)
        {
            OnVantageAreaEntered(this, other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (OnVantageAreaExited != null)
        {
            OnVantageAreaExited(this, other);
        }
    }
}
