using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class InteractionObject : MonoBehaviour
{
    // Event class for interacting with objects
    [System.Serializable]
    public class ObjectInteractionEvent : UnityEvent<InteractionObject> { }

    // Whether or not the object should be brought up into inspection mode
    // Use this for small items like a rat, but not a door.
    public bool isInspectable = true;

    public Vector3 interactionRotation;
    public DialogCard objectDialog;
    public Conversation objectConversation;
    
    public bool isUseable = true;
    public bool disableAfterUse = true;
    public bool deleteAfterUse = false;

    // The minimum distance at which the player can interact with this object
    public float interactionDistance = 4f;

    // Put a prefab here if you want to use another object as the inspected object
    public GameObject objectToInspect;

    // Events that that trigger at the start and end of an interaction
    public ObjectInteractionEvent onInteractionBegin;
    public ObjectInteractionEvent onInteractionEnd;

    public bool Clicked { get; private set; }

    private Material[] materials;
    private InteractionCamera interactionCamera;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        materials = GetComponent<MeshRenderer>().materials;
        SetHoverEffect(false);
        interactionCamera = FindObjectOfType<InteractionCamera>();

        if (objectToInspect == null)
            objectToInspect = this.gameObject;
    }

    float DistanceToPlayer { get { return (player.transform.position - this.transform.position).magnitude; } }
    bool CanInteract { get { return isUseable && DistanceToPlayer < interactionDistance && !interactionCamera.IsDisplaying(); } }

    void OnMouseOver()
    {
        if (CanInteract)
            SetHoverEffect(true);
    }

    void OnMouseExit()
    {
        SetHoverEffect(false);
    }

    void OnMouseDown()
    {
        if (CanInteract)
        {
            interactionCamera.DisplayObject(this);
            Clicked = true;
            SetHoverEffect(false);
            if (disableAfterUse)
                isUseable = false;
        }
    }

    void SetHoverEffect(bool enabled)
    {
        foreach (Material material in materials)
            if (material.HasProperty("_OverlayAmt"))
                material.SetFloat("_OverlayAmt", enabled ? 0.2f : 0f);
    }
}