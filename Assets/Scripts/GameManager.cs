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
    
    public Room GetRandomRoomOfType(RoomType roomType)
    {
        var rooms = roomsPerType.First(r => r.roomType == roomType).rooms;
        return rooms[UnityEngine.Random.Range(0, rooms.Count)];
    }
}

[Serializable]
public struct RoomsPerType
{
    public RoomType roomType;
    public List<Room> rooms;
}
