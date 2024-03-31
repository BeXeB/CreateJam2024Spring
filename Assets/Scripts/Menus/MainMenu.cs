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
        AudioMananger.instance.PlayMusicClip("Non Combat Music");
    }
    public void StartGame()
    {
        AudioMananger.instance.PlayAudioClip("Menu Button Sound");
        SceneManager.LoadScene(firstSceneName);
    }
    public void GoToControlsMenu()
    {
        AudioMananger.instance.PlayAudioClip("Menu Button Sound");
        mainMainMenu.SetActive(false);

        controlsMenu.SetActive(true);
    }
    public void GoToMainMenu()
    {
        AudioMananger.instance.PlayAudioClip("Menu Button Sound");
        controlsMenu.SetActive(false);

        mainMainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        AudioMananger.instance.PlayAudioClip("Menu Button Sound");
        Application.Quit();
    }
}
