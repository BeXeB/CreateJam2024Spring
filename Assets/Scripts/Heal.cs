using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Heal : MonoBehaviour
{
    public delegate void OnCooldownChanged(float currentRecharge, float maxRecharge);
    
    public event OnCooldownChanged onCooldownChanged;
    [SerializeField] private float healAmount = 10f;
    [SerializeField] private float cooldown = 5f;
    
    private PlayerControls playerControls;
    private float cooldownLeft = 0f;
    private Player player = null;

    private void Awake()
    {
        playerControls = new PlayerControls();
        player = gameObject.GetComponent<Player>();
    }
    
    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Heal.performed += TryHeal;
    }
    
    private void OnDisable()
    {
        playerControls.Player.Heal.performed -= TryHeal;
        playerControls.Disable();
    }

    private void Start()
    {
        onCooldownChanged?.Invoke(cooldownLeft, cooldown);
    }

    private void Update()
    {
        if (cooldownLeft > 0)
        {
            cooldownLeft -= Time.deltaTime;
            onCooldownChanged?.Invoke(cooldownLeft, cooldown);
        }
    }
    
    private void TryHeal(InputAction.CallbackContext obj)
    {
        if (cooldownLeft > 0) return;
        player.Heal(healAmount);
        cooldownLeft = cooldown;
    }
}
