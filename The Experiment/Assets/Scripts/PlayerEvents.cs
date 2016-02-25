using UnityEngine;
using System.Collections;

public class PlayerEvents : MonoBehaviour 
{
    public AudioSource audio;

    public void FootStep()
    {
        audio.Play();
    }
}
