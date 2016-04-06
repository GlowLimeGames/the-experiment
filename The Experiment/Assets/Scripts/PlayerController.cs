using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private GameState gameState;
    private bool inRangeInteract = false;
    private PlayerMovementWASD p_Movement;

    void Start()
    {
        p_Movement = GetComponent<PlayerMovementWASD>();
        gameState = FindObjectOfType<GameState>();
    }

    void FixedUpdate()
    {
        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (!gameState.IsGameplayPaused)
        {
            p_Movement.Move(h, v);
        }
        else
        {
            p_Movement.Move(0, 0);
        }
    }
}
