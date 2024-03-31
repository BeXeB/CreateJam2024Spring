using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;

public class Enemy : MonoBehaviour, IClearable
{
    public event OnCleared OnCleared;

    [SerializeField] private float maxHealth = 100f;
    //[SerializeField] private float startingHealth = 100f;
    [SerializeField] private float damage = 10f;
    private float health;
    private Player player;
    [SerializeField] private float attackSpeed = 1f;
    private float attackCooldown = 0f;

    private void Awake()
    {
        health = maxHealth;
        player = GameManager.instance.player.GetComponent<Player>();
        var seeker = GetComponent<AIDestinationSetter>();
        seeker.target = player.transform;
    }

    private void Update()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }
    
    public void Attack(Collision collision)
    {
        if (attackCooldown > 0) return;
        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.Damage(damage);
        }
        attackCooldown = attackSpeed;
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            Attack(other);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag($"Bullet"))
        {
            var bullet = other.GetComponent<Bullet>();
            if (bullet == null) return;
            Damage(bullet.gunController.damage);
            bullet.ReturnToPool();
        }
    }

    public void OnCollisionStay(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            Attack(other);
        }
    }
}
