﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Tooltip("Gameobject to follow.")]
    public CameraTarget target;
    
    [Tooltip("Determines how fast/slow the camera follow the player.")]
    [Range(0f, 1f)]
    public float followSpeed;

    [Tooltip("Use fixed update? (Important for syncing with physics)")]
    public bool useFixedUpdate;

    [Tooltip("Spherical coordinates for the camera to start at.")]
    public float theta, phi, rho;

    [Tooltip("The how much we can zoom in and out.")]
    public float minZoom, maxZoom;

    [Tooltip("How steep the view is at min zoom and max zoom respectively (radians).")]
    [Range(0, Mathf.PI / 2)]
    public float minTheta = Mathf.PI / 2, maxTheta = Mathf.PI / 4;

    void UpdatePosition()
    {
        Vector3 position = target.GetTargetCameraPosition();

        // Set look direction towards the player
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.transform.position - this.transform.position), followSpeed);

        // Move towards target transform
        transform.position = Vector3.Lerp(transform.position, position, followSpeed);
    }

    void Update()
    {
        // Zoom in/out
        target.AdjustRho(Input.mouseScrollDelta.y);
        
        if (Input.GetKey(KeyCode.Q))
            target.AdjustPhi(-Time.deltaTime);
        if (Input.GetKey(KeyCode.E))
            target.AdjustPhi(Time.deltaTime);

        if (!useFixedUpdate) UpdatePosition();   
    }

    void FixedUpdate()
    {
        if (useFixedUpdate) UpdatePosition();   
    }
}
