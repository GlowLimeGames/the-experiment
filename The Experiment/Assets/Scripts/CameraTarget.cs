using UnityEngine;
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

    [Tooltip("Whether or not to use this target's update speed instead of camera defaults.")]
    public bool setUpdateSpeed;

    [Tooltip("Whether the camera should lerp to the desired look angle or snap to it.")]
    public bool noLookLerp;

    [Tooltip("How fast the camera updates towards the target.")]
    public float cameraPositionUpdateSpeed, cameraAngleUpdateSpeed;

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
        while (phi < 0) phi += Mathf.PI * 2;
        while (phi > Mathf.PI * 2) phi -= Mathf.PI * 2;
    }

    public void AdjustTheta(float deltaTheta)
    {
        if (!controllable) return;
        theta += deltaTheta;
        theta = Mathf.Clamp(theta, 0.2f, Mathf.PI * 0.75f);
    }
}
