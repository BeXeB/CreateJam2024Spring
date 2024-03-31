using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    private GameManager gameManager;
    
    [SerializeField] private Room thisRoom;
    [SerializeField] private Collider col;
    [SerializeField] private Image nextRoomIcon;
    [SerializeField] private List<RoomTypeIcon> roomTypeIcons;
    
    private Room nextRoom = null;
    private RoomType nextRoomType;
    
    private void OnEnable()
    {
        gameManager = GameManager.instance;
        col.enabled = false;
        
        nextRoomIcon.gameObject.SetActive(false);
    }

    public void OpenDoor()
    {
        // Play animation
        
        (nextRoom, nextRoomType) = gameManager.GetRandomRoom();
        
        var icon = roomTypeIcons.Find(r => r.roomType == nextRoomType).icon;
        nextRoomIcon.gameObject.SetActive(true);
        nextRoomIcon.sprite = icon;
        
        col.enabled = true;
    }
    
    public void PlayerEnter()
    {
        col.enabled = false;

        thisRoom.gameObject.SetActive(false);
        gameManager.currentRoom = nextRoom;
        nextRoom.gameObject.SetActive(true);
    }
}

[Serializable]
public struct RoomTypeIcon
{
    public RoomType roomType;
    public Sprite icon;
}
