using System.Collections.Generic;
using Enemies;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    public RoomType roomType;
    public Transform playerSpawnPoint;
    
    private List<IEnemy> enemies;
    
    public Door[] doors;

    private GameManager gameManager;
    
    public float2 center;
    public float2 size;
    
    public List<EnemyAmount> enemyAmounts;
    
    private void OnEnable()
    {
        gameManager = GameManager.instance;
        gameManager.player.transform.position = playerSpawnPoint.position;
    }

    private void Start()
    {
        foreach (var enemyAmount in enemyAmounts)
        {
            for (var i = 0; i < enemyAmount.amount; i++)
            {
                var enemy = Instantiate(enemyAmount.enemyPrefab, GetRandomPosition(), quaternion.identity);
                var enemyComponent = enemy.GetComponent<IEnemy>();
                enemies.Add(enemyComponent);
                enemy.GetComponent<IEnemy>().OnDeath += OnEnemyDeath;
            }
        }
    }
    
    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(center.x - size.x / 2, center.x + size.x / 2),
            Random.Range(center.y - size.y / 2, center.y + size.y / 2), 0);
    }
    
    private void OnEnemyDeath(IEnemy enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            // Open doors
        }
    }
}

[System.Serializable]
public struct EnemyAmount
{
    public GameObject enemyPrefab;
    public int amount;
}

public enum RoomType
{
    Start,
    Normal,
}
