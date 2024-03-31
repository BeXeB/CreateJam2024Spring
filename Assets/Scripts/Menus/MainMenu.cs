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

    private void Start()
    {
        AudioManager.instance.PlayAudioClip("Non Combat Music");
    }
    public void StartGame()
    {
        AudioManager.instance.PlayAudioClip("Menu Button Sound");
        SceneManager.LoadScene(firstSceneName);
    }
    public void GoToControlsMenu()
    {
        AudioManager.instance.PlayAudioClip("Menu Button Sound");
        mainMainMenu.SetActive(false);

        controlsMenu.SetActive(true);
    }
    public void GoToMainMenu()
    {
        AudioManager.instance.PlayAudioClip("Menu Button Sound");
        controlsMenu.SetActive(false);

        mainMainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        AudioManager.instance.PlayAudioClip("Menu Button Sound");
        Application.Quit();
    }
}
