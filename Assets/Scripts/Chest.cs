using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IClearable
{
    [SerializeField] private Conversation conversation;
    public void Open()
    {
        var itemobtained = Random.Range(0, 2) == 0 ? "Health Potion" : "Mana Potion";
        conversation.SetDialogue(new List<DialogueLine>
        {
            new () {message = "You found a " + itemobtained + "!"}
        });
        
        conversation.SetUpConversation();
        
        OnCleared?.Invoke(this);
    }

    public event OnCleared OnCleared;
}
