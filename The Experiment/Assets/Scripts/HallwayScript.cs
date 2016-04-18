using UnityEngine;
using System.Collections;

public class HallwayScript : MonoBehaviour
{
    public Transform startRoomSpawnLocation;
    public Transform recordsRoomSpawnLocation;
    
    public void PlacePlayerAtStartRoomDoor()
    {
        var player = FindObjectOfType<PlayerController>();
        var cam = FindObjectOfType<CameraFollow>();
        player.transform.position = startRoomSpawnLocation.position;
        player.transform.rotation = startRoomSpawnLocation.rotation;
        cam.JumpToTarget();
    }

    public void PlacePlayerAtRecordsRoomDoor()
    {
        var player = FindObjectOfType<PlayerController>();
        var cam = FindObjectOfType<CameraFollow>();
        player.transform.position = recordsRoomSpawnLocation.position;
        player.transform.rotation = recordsRoomSpawnLocation.rotation;
        cam.JumpToTarget();
    }
}
