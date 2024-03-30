using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IClearable
{
    public event OnCleared OnCleared;

    [SerializeField] private float maxHealth = 100f;
    //[SerializeField] private float startingHealth = 100f;
    [SerializeField] private float damage = 10f;
    private float health;

    private void Awake()
    {
        health = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(Collision collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.Damage(damage);
        }
    }

    public void Damage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnCleared?.Invoke(this);
        Destroy(gameObject);
    }
}
