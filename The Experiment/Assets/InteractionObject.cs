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

    public bool isUseable = true;
    public bool disableAfterUse = true;

    private Material[] materials;
    private InteractionCamera interactionCamera;

    public bool Clicked { get; private set; }

    void Start()
    {
        materials = GetComponent<MeshRenderer>().materials;
        SetHoverEffect(false);
        interactionCamera = FindObjectOfType<InteractionCamera>();
    }

    void OnMouseOver()
    {
        if (isUseable)
            SetHoverEffect(true);
    }

    void OnMouseExit()
    {
        if (isUseable)
            SetHoverEffect(false);
    }

    void OnMouseDown()
    {
        if (isUseable && !interactionCamera.IsDisplaying())
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