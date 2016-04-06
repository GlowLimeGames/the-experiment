using UnityEngine;
using System.Collections;

public class InteractionObject : MonoBehaviour
{
    // Whether or not the object should be brought up into inspection mode
    public bool isInspectable = true;

    public Vector3 interactionRotation;
    public DialogCard objectDialog;

    // The objects that will recieve a RunBehavior message after interaction
    public GameObject[] interactionObjects;

    public MeshRenderer outlineRenderer;

    public bool isUseable = true;
    public bool disableAfterUse = true;

    private InteractionCamera interactionCamera;

    public bool Clicked { get; private set; }

    void Start()
    {
        outlineRenderer.enabled = false;
        interactionCamera = FindObjectOfType<InteractionCamera>();
    }

    void OnMouseOver()
    {
        if (isUseable && outlineRenderer != null)
            outlineRenderer.enabled = true;
    }

    void OnMouseExit()
    {
        if (isUseable && outlineRenderer != null)
            outlineRenderer.enabled = false;
    }

    void OnMouseDown()
    {
        if (isUseable && !interactionCamera.IsDisplaying())
        {
            interactionCamera.DisplayObject(this);
            Clicked = true;
            if (disableAfterUse)
                isUseable = false;
        }
    }
}