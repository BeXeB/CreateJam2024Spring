using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    public RoomType roomType;
    public Transform playerSpawnPoint;
    
    private List<IClearable> enemies;
    
    public Door[] doors;

    private GameManager gameManager;
    
    public List<SpawnArea> spawnAreas;
    
    public List<EnemyAmount> enemyAmounts;
    
    private void OnEnable()
    {
        gameManager = GameManager.instance;
        gameManager.player.transform.position = playerSpawnPoint.position;
        enemies = new List<IClearable>();
        
        foreach (var enemyAmount in enemyAmounts)
        {
            for (var i = 0; i < enemyAmount.amount; i++)
            {
                var enemy = Instantiate(enemyAmount.enemyPrefab, GetRandomPosition(), quaternion.identity);
                var enemyComponent = enemy.GetComponent<IClearable>();
                enemies.Add(enemyComponent);
                enemy.GetComponent<IClearable>().OnCleared += OnCleared;
            }
        }
    }
    
    private Vector3 GetRandomPosition()
    {
        var spawnArea = spawnAreas[Random.Range(0, spawnAreas.Count)];
        var position = spawnArea.center.position;
        var randomPosition = new Vector3(Random.Range(position.x - spawnArea.size.x / 2, position.x + spawnArea.size.x / 2),
            1f, Random.Range(position.z - spawnArea.size.y / 2, position.z + spawnArea.size.y / 2));
        return randomPosition;
    }
    
    private void OnCleared(IClearable clearable)
    {
        enemies.Remove(clearable);
        if (enemies.Count != 0) return;
        foreach (var door in doors)
        {
            door.OpenDoor();
        }
    }
}

[Serializable]
public struct EnemyAmount
{
    public GameObject enemyPrefab;
    public int amount;
}

[Serializable]
public struct SpawnArea
{
    public Transform center;
    public float2 size;
}

public enum RoomType
{
    Normal,
}
