using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float startingHealth = 100f;
    private float health;

    [Header("Movement")]
    [SerializeField] private InputAction playerControls;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float movementSpeed = 5f;
    private Vector2 moveDirection;

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Awake()
    {
        health = startingHealth; //TODO if we reload the scripts into new scenes, we need a variable for storing the health
    }

    public void Heal(float amount)
    {
        health += amount;

        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void Damagage(float amount)
    {
        health -= amount;

        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = playerControls.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = moveDirection * movementSpeed;
    }
}
