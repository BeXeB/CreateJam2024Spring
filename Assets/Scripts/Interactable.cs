using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private List<UnityEvent> onInteractEvents;
    
    public void Interact()
    {
        Debug.Log("Interacting");
        foreach (var onInteractEvent in onInteractEvents)
        {
            onInteractEvent.Invoke();
        }
    }
}
