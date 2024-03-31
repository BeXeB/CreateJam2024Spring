using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public delegate void OnHealthChanged(float currentHealth, float maxHealth);
    public event OnHealthChanged onHealthChanged;
    
    
    public string playerName = "Player"; // TODO choose a character name or let player type one
    public List<AudioClip> voicePool; // Pool used for playing sound when player speaks in dialogue
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float startingHealth = 100f;
    private float health;

    [Header("Movement")]
    internal PlayerControls playerControls;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] internal float movementSpeed = 5f;
    private Vector2 moveDirection;
    private bool isLocked = false;

    [SerializeField] private GameObject playerCamera;
    internal float movementSpeedShootingReduction = 1;

    private Interactable nearbyInteractable;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        health = startingHealth;
        onHealthChanged?.Invoke(health, maxHealth);
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Interact.performed += Interact;
    }

    private void OnDisable()
    {
        playerControls.Player.Interact.performed -= Interact;
        playerControls.Disable();
    }

    public void Heal(float amount)
    {
        health += amount;
        onHealthChanged?.Invoke(health, maxHealth);

        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void Damage(float amount)
    {
        health -= amount;
        onHealthChanged?.Invoke(health, maxHealth);

        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene(3);
    }

    private void Update()
    {
        if(isLocked)
        {
            moveDirection = Vector2.zero;
        }
        else
        {
            moveDirection = playerControls.Player.Move.ReadValue<Vector2>();
        }
    }

    private void FixedUpdate()
    {
        var moveDir = new Vector3(moveDirection.x, 0, moveDirection.y);
        moveDir = playerCamera.transform.TransformDirection(moveDir);
        rigidBody.position += moveDir * (movementSpeed * movementSpeedShootingReduction * Time.fixedDeltaTime);
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (nearbyInteractable)
        {
            nearbyInteractable.Interact();
        }
    }

    public void Lock()
    {
        isLocked = true;
    }

    public void Free()
    {
        isLocked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() != null)
        {
            nearbyInteractable = other.gameObject.GetComponent<Interactable>();
            //interactCanvas.gameObject.SetActive(true); TODO this if neccessary
        }
        if (other.gameObject.CompareTag("Door"))
        {
            var door = other.gameObject.GetComponent<Door>();
            
            door.PlayerEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() == nearbyInteractable)
        {
            //interactCanvas.gameObject.SetActive(false); TODO this if neccessary
            nearbyInteractable = null;
        }
    }
}
