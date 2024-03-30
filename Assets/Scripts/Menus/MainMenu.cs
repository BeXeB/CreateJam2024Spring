using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMainMenu;
    [SerializeField] private string firstSceneName = "SampleScene";
    [SerializeField] private GameObject controlsMenu;
    
    public void StartGame()
    {
        SceneManager.LoadScene(firstSceneName);
    }
    public void GoToControlsMenu()
    {
        mainMainMenu.SetActive(false);

        controlsMenu.SetActive(true);
    }
    public void GoToMainMenu()
    {
        controlsMenu.SetActive(false);

        mainMainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
