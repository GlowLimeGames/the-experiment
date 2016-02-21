﻿using UnityEngine;
using System.Collections;

// Stores information about how the camera focuses on this target
public class CameraTarget : MonoBehaviour
{
    [Tooltip("Spherical coordinates in world space about this target.")]
    public float rho, theta, phi;

    [Tooltip("Should the camera always be behind the target?")]
    public bool followBehind;

    [Tooltip("Whether we can manipulate the camera while looking at this target.")]
    public bool controllable;

    public Vector3 GetTargetCameraPosition()
    {
        if (followBehind) phi = Mathf.Atan2(this.transform.forward.z, this.transform.forward.x) + Mathf.PI;

        float x = rho * Mathf.Sin(theta) * Mathf.Cos(phi);
        float y = rho * Mathf.Cos(theta);
        float z = rho * Mathf.Sin(theta) * Mathf.Sin(phi);
        return new Vector3(x, y, z) + transform.position;
    }

    public void AdjustRho(float deltaRho)
    {
        if (!controllable) return;
        rho += deltaRho;
    }

    public void AdjustPhi(float deltaPhi)
    {
        if (!controllable) return;
        phi += deltaPhi;
    }

    public void AdjustTheta(float deltaTheta)
    {
        if (!controllable) return;
        theta += deltaTheta;
    }
}