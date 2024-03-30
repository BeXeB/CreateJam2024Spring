using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AttachmentMenu : MonoBehaviour
{
    [SerializeField] private GameObject attachmentMenu;
    [FormerlySerializedAs("detachButton")] [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject attachmentButtonPrefab;
    private GunController gunController;

    private void Start()
    {
        gunController = FindAnyObjectByType<GunController>();
    }

    public void ShowAttachment(int attachmentTypeIndex)
    {
        foreach(Transform child in attachmentMenu.transform)
        {
            Destroy(child.gameObject);
        }
        
        backButton.SetActive(true);
        
        attachmentMenu.SetActive(true);

        switch((AttachmentType)attachmentTypeIndex)
        {
            case AttachmentType.Scope:
                List<Scope> scopes = gunController.attachmentsInInventory.FindAll(attachment => attachment.attachmentType == AttachmentType.Scope).ConvertAll(attachment => (Scope)attachment);
                
                foreach(Scope scope in scopes)
                {
                    GameObject attachmentButton = Instantiate(attachmentButtonPrefab, attachmentMenu.transform);
                    attachmentButton.GetComponent<AttachmentUIElement>().Initialize(scope);
                }
                break;
            case AttachmentType.Barrel:
                List<Barrel> barrels = gunController.attachmentsInInventory.FindAll(attachment => attachment.attachmentType == AttachmentType.Barrel).ConvertAll(attachment => (Barrel)attachment);
                break;
            case AttachmentType.Magazine:
                List<Magazine> magazines = gunController.attachmentsInInventory.FindAll(attachment => attachment.attachmentType == AttachmentType.Magazine).ConvertAll(attachment => (Magazine)attachment);
                break;
            case AttachmentType.Stock:
                List<Stock> stocks = gunController.attachmentsInInventory.FindAll(attachment => attachment.attachmentType == AttachmentType.Stock).ConvertAll(attachment => (Stock)attachment);
                break;
            case AttachmentType.Receiver:
                List<Receiver> receivers = gunController.attachmentsInInventory.FindAll(attachment => attachment.attachmentType == AttachmentType.Receiver).ConvertAll(attachment => (Receiver)attachment);
                break;
            case AttachmentType.Catalyst:
                List<Catalyst> catalysts = gunController.attachmentsInInventory.FindAll(attachment => attachment.attachmentType == AttachmentType.Catalyst).ConvertAll(attachment => (Catalyst)attachment);
                break;
        }
    }

    public void BackButton()
    {
        attachmentMenu.SetActive(false);
        backButton.SetActive(false);
    }
}
