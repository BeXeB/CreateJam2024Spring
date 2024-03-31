using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private Player player;
    
    public Slider healthBar;

    private void Awake()
    {
        player = GameManager.instance.player.GetComponent<Player>();
        player.onHealthChanged += UpdateHealthBar;
    }
    
    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBar.value = currentHealth / maxHealth;
    }
}
