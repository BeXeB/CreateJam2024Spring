using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealUI : MonoBehaviour
{
    private Heal heal;
    [SerializeField] private Image cooldownBar;

    private void Awake()
    {
        heal = GameManager.instance.player.GetComponent<Heal>();
        heal.onCooldownChanged += UpdateCooldownBar;
    }

    private void UpdateCooldownBar(float currentRecharge, float maxRecharge)
    {
        Debug.Log(currentRecharge);
        if (currentRecharge > 0)
        {
            cooldownBar.color = new Color(0, 0, 0, 0.8f);
            return;
        }
        cooldownBar.color = new Color(1, 1, 1, 0);
    }
}
