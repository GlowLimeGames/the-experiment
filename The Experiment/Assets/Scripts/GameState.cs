using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour 
{
    private GameMenu gameMenu;
    private DialogBox dialog;
    private Grayscale grayscale;

	void Awake() 
    {
        dialog = FindObjectOfType<DialogBox>();
        grayscale = FindObjectOfType<Grayscale>();
        gameMenu = FindObjectOfType<GameMenu>();
	}

    public bool IsGameplayPaused
    {
        get 
        { 
            bool paused = false;
            if (dialog != null)
                paused = paused || dialog.IsDisplaying();
            if (gameMenu != null)
                paused = paused || gameMenu.IsActive;
            return paused;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (dialog == null || !dialog.IsDisplaying()))
        {
            if(gameMenu != null)
                gameMenu.Toggle();
        }
    }
}
