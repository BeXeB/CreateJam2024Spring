using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    private GameManager gameManager;
    
    [SerializeField] private Room thisRoom;
    [SerializeField] private Collider col;
    private Room nextRoom = null;
    
    private void OnEnable()
    {
        gameManager = GameManager.instance;
        col.enabled = false;
    }

    public void OpenDoor()
    {
        // Play animation
        
        var roomType = Enum.GetValues(typeof(RoomType));
        var randomRoomType = (RoomType) roomType.GetValue(UnityEngine.Random.Range(0, roomType.Length));
        nextRoom = gameManager.GetRandomRoomOfType(randomRoomType);
        
        // Display next room icon
        
        col.enabled = true;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        col.enabled = false;

        thisRoom.gameObject.SetActive(false);
        nextRoom.gameObject.SetActive(true);
    }
}
