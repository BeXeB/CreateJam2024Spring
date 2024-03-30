using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private string firstSceneName = "SampleScene";
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public void RestartGame()
    {
        SceneManager.LoadScene(firstSceneName);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
