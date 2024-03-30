using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttachmentUIElement : MonoBehaviour
{
    private GunController gunController;
    public Attachment attachment;
    public Image uiImage;
    public TMP_Text uiText;

    private void Start()
    {
        gunController = FindAnyObjectByType<GunController>();
    }

    public void Initialize(Attachment attachment)
    {
        this.attachment = attachment;
        uiImage.sprite = attachment.attachmentImage;
        uiText.text = attachment.name;
    }

    public void OnClick()
    {
        switch(attachment.attachmentType)
        {
            case AttachmentType.Scope:
                if(gunController.equippedScope != null) gunController.DeEquipAttachment(gunController.equippedScope);
                break;
            case AttachmentType.Barrel:
                if(gunController.equippedBarrel != null) gunController.DeEquipAttachment(gunController.equippedBarrel);
                break;
            case AttachmentType.Magazine:
                if(gunController.equippedMagazine != null) gunController.DeEquipAttachment(gunController.equippedMagazine);
                break;
            case AttachmentType.Stock:
                if(gunController.equippedStock != null) gunController.DeEquipAttachment(gunController.equippedStock);
                break;
            case AttachmentType.Receiver:
                if(gunController.equippedReceiver != null) gunController.DeEquipAttachment(gunController.equippedReceiver);
                break;
            case AttachmentType.Catalyst:
                if(gunController.equippedCatalyst != null) gunController.DeEquipAttachment(gunController.equippedCatalyst);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        gunController.EquipAttachment(attachment);
        
        transform.parent.parent.GetComponent<AttachmentMenu>().BackButton();
    }
}
