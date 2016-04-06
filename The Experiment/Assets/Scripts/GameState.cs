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
        get { return dialog.IsDisplaying() || gameMenu.IsActive; }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&& !dialog.IsDisplaying())
        {
            gameMenu.Toggle();
        }
    }
}
