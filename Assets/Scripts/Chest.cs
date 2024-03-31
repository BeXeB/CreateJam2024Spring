using System;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IClearable
{
    [SerializeField] private Conversation conversation;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    public void Open()
    {
        var attachment = gameManager.GetRandomAttachment();
        
        if (attachment == null)
        {
            conversation.SetDialogue(new List<DialogueLine>
            {
                new () {message = "You found all attachments!"}
            });
            
            conversation.SetUpConversation();
            
            OnCleared?.Invoke(this);
            return;
        }
        
        conversation.SetDialogue(new List<DialogueLine>
        {
            new () {message = "You found a " + attachment.name + "!"}
        });
        
        gameManager.player.GetComponent<GunController>().AddAttachment(attachment);
        
        conversation.SetUpConversation();
        
        OnCleared?.Invoke(this);
    }

    public event OnCleared OnCleared;
}
