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
    internal PlayerControls playerControls;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float movementSpeed = 5f;
    private Vector2 moveDirection;
    [SerializeField] private GameObject playerCamera;

    private void Awake()
    {
        playerControls = new PlayerControls();
        health = startingHealth;
    }
    
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void Heal(float amount)
    {
        health += amount;

        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void Damage(float amount)
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

    private void Update()
    {
        moveDirection = playerControls.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        var moveDir = new Vector3(moveDirection.x, 0, moveDirection.y);
        moveDir = playerCamera.transform.TransformDirection(moveDir);
        rigidBody.position += moveDir * (movementSpeed * Time.fixedDeltaTime);
    }
}
