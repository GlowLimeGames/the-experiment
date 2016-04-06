using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour 
{
    private Grayscale grayscale;
    private DialogBox dialog;

    void Start()
    {
        grayscale = FindObjectOfType<Grayscale>();
        dialog = FindObjectOfType<DialogBox>();
        gameObject.SetActive(false);
    }

    public bool IsActive
    {
        get { return gameObject.activeSelf; }
    }

    public void Toggle()
    {
        if (IsActive) Resume();
        else Pause();
    }

    public void Pause()
    {
        gameObject.SetActive(true);
        grayscale.effectAmount = 1;
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        grayscale.effectAmount = 0;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
