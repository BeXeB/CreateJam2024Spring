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
    
    public Room currentRoom;
    
    public Room GetRandomRoomOfType(RoomType roomType)
    {
        var rooms = roomsPerType.First(r => r.roomType == roomType).rooms;
        var availableRooms = rooms.Where(r => r != currentRoom).ToArray();
        return availableRooms[UnityEngine.Random.Range(0, availableRooms.Length)];
    }
}

[Serializable]
public struct RoomsPerType
{
    public RoomType roomType;
    public List<Room> rooms;
}
