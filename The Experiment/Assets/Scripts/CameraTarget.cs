using UnityEngine;
using System.Collections;

// Stores information about how the camera focuses on this target
public class CameraTarget : MonoBehaviour
{
    public float rho, theta, phi;

    public bool followBehind;

    public Vector3 GetTargetCameraPosition()
    {
        if (followBehind) rho = Vector3.Angle(Vector3.left, this.transform.forward);



        return Vector3.zero;
    }
}
