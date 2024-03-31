using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    public GameObject player;
    [SerializeField] private List<RoomsPerType> roomsPerType;

    [SerializeField] private List<Attachment> unlockableAttachments;
    
    public Room currentRoom;
    
    public (Room, RoomType) GetRandomRoom()
    {
        while (true)
        {
            var roomType = Enum.GetValues(typeof(RoomType));
            var randomRoomType = (RoomType) roomType.GetValue(UnityEngine.Random.Range(0, roomType.Length));
            var rooms = roomsPerType.Find(r => r.roomType == randomRoomType).rooms;
            var availableRooms = rooms.Where(r => r != currentRoom).ToArray();
        
            if (availableRooms.Length == 0) continue;
            
            return (availableRooms[UnityEngine.Random.Range(0, availableRooms.Length)], randomRoomType);
        }
    }
    
    public Attachment GetRandomAttachment()
    {
        if (unlockableAttachments.Count == 0) return null;
        var attachment = unlockableAttachments[UnityEngine.Random.Range(0, unlockableAttachments.Count)];
        unlockableAttachments.Remove(attachment);
        return attachment;
    }
}

[Serializable]
public struct RoomsPerType
{
    public RoomType roomType;
    public List<Room> rooms;
}
