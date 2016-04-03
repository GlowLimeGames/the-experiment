using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HallwayOpen : MonoBehaviour
{
    void RunBehavior()
    {
        SceneManager.LoadScene("FirstHallway");
    }
}
