using UnityEngine;
using System.Collections;

public class HallwayScript : MonoBehaviour
{
    public Transform startRoomSpawnLocation;
    public Transform recordsRoomSpawnLocation;
    public Conversation enterHallwayConvo;
        
    public void PlacePlayerAtStartRoomDoor()
    {
        StartCoroutine(PlacePlayerAtStartRoomDoorCoroutine());
    }

    private IEnumerator PlacePlayerAtStartRoomDoorCoroutine()
    {
        var player = FindObjectOfType<PlayerController>();
        var cam = FindObjectOfType<CameraFollow>();
        var grayscale = FindObjectOfType<Grayscale>();
        var conversationManager = FindObjectOfType<ConversationManager>();

        player.transform.position = startRoomSpawnLocation.position;
        player.transform.rotation = startRoomSpawnLocation.rotation;
        cam.JumpToTarget();

        yield return grayscale.Fade(false, 3f);
                
        conversationManager.RunConversation(enterHallwayConvo);
    }

    public void PlacePlayerAtRecordsRoomDoor()
    {
        var player = FindObjectOfType<PlayerController>();
        var cam = FindObjectOfType<CameraFollow>();
        var grayscale = FindObjectOfType<Grayscale>();

        player.transform.position = recordsRoomSpawnLocation.position;
        player.transform.rotation = recordsRoomSpawnLocation.rotation;
        cam.JumpToTarget();
        grayscale.Fade(false, 3f);
    }
}
