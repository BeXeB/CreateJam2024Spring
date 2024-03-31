using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    private GameManager gameManager;
    
    [SerializeField] private Room thisRoom;
    [SerializeField] private Collider col;
    private Room nextRoom = null;
    private RoomType nextRoomType;
    
    private void OnEnable()
    {
        gameManager = GameManager.instance;
        col.enabled = false;
    }

    public void OpenDoor()
    {
        // Play animation
        
        (nextRoom, nextRoomType) = gameManager.GetRandomRoom();
        
        // Display next room icon
        
        col.enabled = true;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        col.enabled = false;

        thisRoom.gameObject.SetActive(false);
        gameManager.currentRoom = nextRoom;
        nextRoom.gameObject.SetActive(true);
    }
}
