using UnityEngine;
using System.Collections;

public class KeyRat : MonoBehaviour 
{
    public Renderer outlineRenderer;

    public bool Clicked { get; private set; }

    void Start()
    {
        outlineRenderer.enabled = false;
    }

    void OnMouseOver()
    {
        outlineRenderer.enabled = true;
    }

    void OnMouseExit()
    {
        outlineRenderer.enabled = false;
    }

    void OnMouseDown()
    {
        Clicked = true;
    }
}
