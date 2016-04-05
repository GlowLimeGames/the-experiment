using UnityEngine;
using System.Collections;

public class SmallInteractionObject : MonoBehaviour
{
    public Vector3 interactionRotation;
    public DialogCard objectDialog;

    // The objects that will recieve a RunBehavior message after interaction
    public GameObject[] interactionObjects;

    public MeshRenderer outlineRenderer;

    public bool isUseable = true;
    public bool disableAfterUse = true;

    private InteractionCameraView interactionCameraView;

    public bool Clicked { get; private set; }

    void Start()
    {
        outlineRenderer.enabled = false;
        interactionCameraView = FindObjectOfType<InteractionCameraView>();
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
        if (isUseable && !interactionCameraView.IsDisplaying())
        {
            interactionCameraView.DisplayObject(this);
            Clicked = true;
            if (disableAfterUse)
                isUseable = false;
        }
    }
}