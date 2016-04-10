﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class InteractionCamera : MonoBehaviour
{
    public float rotationRate;
    public float inflationFactor;

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

    public void DisplayObject(InteractionObject interactionObject)
    {
        if (interactionObject.objectDialog != null)
            dialog.SetDialogQueue(interactionObject.objectDialog);
        dialog.DisplayNextCard();

        if (interactionObject.isInspectable)
        {
            interactionObject.isUseable = false;

            currentInspectedObject = (GameObject)Instantiate(interactionObject.gameObject, this.transform.position, Quaternion.Euler(interactionObject.interactionRotation));
            currentInspectedObject.transform.parent = this.transform;

            PlaceObjectToFit();

            camera.enabled = true;
            cameraGrayscale.effectAmount = 1;
        }

        StartCoroutine(CloseInteractionCamera(interactionObject.interactionObjects));
    }

    void PlaceObjectToFit()
    {
        // Don't question the math
        var size = Vector3.Scale(currentInspectedObject.GetComponent<MeshFilter>().mesh.bounds.size, currentInspectedObject.transform.localScale);

        // Artificially inflate size to make is scale into a smaller box
        size *= inflationFactor;

        float height = Mathf.Max(size.x, size.z, size.y);
        float width = Mathf.Max(size.x, size.z, size.y);
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
            distance = width + camera.nearClipPlane;


        currentInspectedObject.transform.position = this.transform.position + this.transform.forward * distance;
    }

    IEnumerator RotateObjectCoroutine()
    {
        while (true)
        {
            while (Input.GetMouseButton(0))
            {
                float x = Input.GetAxis("Mouse X");
                float y = Input.GetAxis("Mouse Y");

                Quaternion currentRotation = currentInspectedObject.transform.rotation;
                Quaternion rotation = Quaternion.Inverse(currentRotation) * Quaternion.Euler(y * rotationRate, -x * rotationRate, 0) * currentRotation;
                currentInspectedObject.transform.rotation *= rotation;
                yield return null;
            }

            yield return null;
        }
    }

    IEnumerator CloseInteractionCamera(GameObject[] interactionObjects)
    {
        Coroutine rotateCoroutine = null;
        if(currentInspectedObject != null)
            rotateCoroutine = StartCoroutine(RotateObjectCoroutine());

        while (dialog.IsDisplaying())
            yield return null;

        if(rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        camera.enabled = false;
        cameraGrayscale.effectAmount = 0;

        // Run world behaviors resulting from interaction
        foreach (GameObject target in interactionObjects)
            target.SendMessage("RunBehavior");

        Destroy(currentInspectedObject);
    }
}