using System;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IClearable
{
    [SerializeField] private Conversation conversation;
    private GameManager gameManager;

    private bool opened = false;
    
    private void Start()
    {
        gameManager = GameManager.instance;
    }

    private void OnEnable()
    {
        opened = false;
    }

    public void Open()
    {
        if (opened) return;
        var attachment = gameManager.GetRandomAttachment();
        
        if (attachment == null)
        {
            conversation.SetDialogue(new List<DialogueLine>
            {
                new () {message = "You found all attachments!"}
            });
            
            conversation.SetUpConversation();
            
            OnCleared?.Invoke(this);
            opened = true;
            return;
        }
        
        conversation.SetDialogue(new List<DialogueLine>
        {
            new () {message = "You found a " + attachment.name + "!"}
        });
        
        gameManager.player.GetComponent<GunController>().AddAttachment(attachment);
        
        conversation.SetUpConversation();
        opened = true;
        OnCleared?.Invoke(this);
    }

    public event OnCleared OnCleared;
}
