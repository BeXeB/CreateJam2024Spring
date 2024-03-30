using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Pathfinding;

public class Conversation : MonoBehaviour
{
    [Header("Entity")]
    [SerializeField] private string entityName;
    [SerializeField] List<AudioClip> entityVoicePool;

    private Player player;
    private List<AudioClip> playerVoicePool;
    private string playerName;

    private PlayerControls playerControls;

    [SerializeField] private List<DialogueLine> dialouge;
    private int currentLine = 0;

    [Header("UI")]
    [SerializeField] Canvas canvas;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogueLineText;

    private AudioSource audioSource;

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Continue.performed += ContinueDialogue;
    }

    private void OnDisable()
    {
        playerControls.Player.Continue.performed -= ContinueDialogue;
        playerControls.Disable();
    }

    private void Awake()
    {
        playerControls = new PlayerControls();
        audioSource = GetComponent<AudioSource>();
        if (!canvas)
        {
            throw new Exception("Missing Conversation Canvas");
        }

        player = FindFirstObjectByType<Player>().GetComponent<Player>();
        playerVoicePool = player.voicePool;
        playerName = player.name;
    }

    private void End()
    {
        player.Free();
        canvas.gameObject.SetActive(false);
        this.enabled = false;
    }

    public void ContinueDialogue(InputAction.CallbackContext context)
    {
        Debug.Log("CONTINUE PRESSED");
        PerformDialogue();
    }

    private void PerformDialogue()
    {
        if (currentLine >= dialouge.Count)
        {
            End();
            return;
        }

        if (dialouge[currentLine].playerMessage)
        {
            speakerText.text = playerName;
            playVoice(playerVoicePool);
        }
        else
        {
            speakerText.text = entityName;
            playVoice(entityVoicePool);
        }
        dialogueLineText.text = dialouge[currentLine].message;
        currentLine++;
    }

    private void playVoice(List<AudioClip> voicePool)
    {
        if (voicePool != null)
        {
            int count = voicePool.Count;
            if (count > 0)
            {
                System.Random rnd = new System.Random();
                audioSource.clip = voicePool[rnd.Next(count - 1)];
                audioSource.Play();
            }
        }
    }

    public void SetUpConversation()
    {
        player.Lock();
        canvas.gameObject.SetActive(true);
        currentLine = 0;
        PerformDialogue();
    }
}
