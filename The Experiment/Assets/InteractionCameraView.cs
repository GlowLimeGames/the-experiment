using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class InteractionCameraView : MonoBehaviour
{
    private DialogBox dialog;
    private Vector3 viewBounds;
    private Grayscale cameraGrayscale;
    private Camera camera;
    private GameObject currentInspectedObject;

    void Start()
    {
        dialog = Object.FindObjectOfType<DialogBox>();
        cameraGrayscale = Object.FindObjectOfType<Grayscale>();
        camera = GetComponent<Camera>();
        camera.enabled = false;
    }

    public bool IsDisplaying()
    {
        return camera.enabled;
    }

    public void DisplayObject(SmallInteractionObject newDisplayObject)
    {
        Destroy(currentInspectedObject);
        newDisplayObject.isUseable = false;

        Debug.Log(newDisplayObject.interactionRotation);
        currentInspectedObject = (GameObject)Instantiate(newDisplayObject.gameObject, this.transform.position, Quaternion.identity);
        currentInspectedObject.transform.localRotation = Quaternion.Euler(newDisplayObject.interactionRotation);
        currentInspectedObject.transform.parent = this.transform;

        PlaceObjectToFit();

        camera.enabled = true;
        cameraGrayscale.enabled = true;
        cameraGrayscale.effectAmount = 1;
        if (newDisplayObject.objectDialog != null)
            dialog.SetDialogQueue(newDisplayObject.objectDialog);
        dialog.DisplayNextCard();

        StartCoroutine(CloseInteractionCamera(newDisplayObject.interactionObjects));
    }

    void PlaceObjectToFit()
    {
        // Don't question the math
        var size = Vector3.Scale(currentInspectedObject.GetComponent<MeshFilter>().mesh.bounds.size, currentInspectedObject.transform.localScale);

        // Artificially inflate size to make is scale into a smaller box
        size *= 2f;

        float height = size.y;
        float width = Mathf.Max(size.x, size.z);
        float objectRatio = width / height;

        float distance = 0f;

        if (objectRatio > camera.aspect)
        {
            // Need to fit width
            float horizontalFov = Mathf.Atan(Mathf.Tan(0.5f * camera.fieldOfView * Mathf.Deg2Rad) * camera.aspect);
            distance = (width * 0.5f) / Mathf.Tan(horizontalFov);
        }
        else
        {
            // Need to fit height
            distance = (height * 0.5f) / Mathf.Tan(Mathf.Deg2Rad * 0.5f * camera.fieldOfView);
        }

        // Make sure item doesn't hit camera clip plane.
        if (distance - camera.nearClipPlane < width)
            width = distance - camera.nearClipPlane;

        currentInspectedObject.transform.position = this.transform.position + this.transform.forward * distance;
    }

    IEnumerator CloseInteractionCamera(GameObject[] interactionObjects)
    {
        while (dialog.IsDisplaying())
            yield return null;

        // Else...
        camera.enabled = false;
        cameraGrayscale.enabled = false;

        // Run world behaviors resulting from interaction
        foreach (GameObject target in interactionObjects)
            target.SendMessage("RunBehavior");
    }
}