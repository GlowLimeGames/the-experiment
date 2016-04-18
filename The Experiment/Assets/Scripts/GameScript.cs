using UnityEngine;
using System.Collections;

/*
 * Manages the state between scenes
 */
public class GameScript : MonoBehaviour 
{
    public static int StartRoom = 1;
    public static int Hallway = 2;
    public static int RecordsRoom = 3;

    private static GameScript instance = null;
    private int previousScene = 1;

    void Awake()
    {
        // Only keep one of these around
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == Hallway)
        {
            HallwayScript hallway = FindObjectOfType<HallwayScript>();

            // Place the player at the appropriate position
            if (previousScene == StartRoom)
                hallway.PlacePlayerAtStartRoomDoor();
            else if (previousScene == RecordsRoom)
                hallway.PlacePlayerAtRecordsRoomDoor();
        }

        previousScene = level;
    }
}
