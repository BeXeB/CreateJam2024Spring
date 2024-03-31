using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMenu : MonoBehaviour
{
    [SerializeField] private GameObject gunMenu;
    private Player player;
    
    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        player.playerControls.Player.GunMenu.performed += ctx => ShowGunMenu();
    }

    private void ShowGunMenu()
    {
        Time.timeScale = 0f;
        gunMenu.SetActive(true);
    }
    
    public void HideGunMenu()
    {
        Time.timeScale = 1f;
        gunMenu.SetActive(false);
    }
}
