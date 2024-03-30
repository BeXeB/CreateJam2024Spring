using UnityEngine;

public abstract class Attachment : MonoBehaviour
{
    protected GunController gunController;
    
    [Header("Attachment")]
    public Sprite attachmentImage;
    public AttachmentType attachmentType;
    
    [Header("Magazine")]
    public MagazineType magazineType;
    public Color magazineColor;
    [Header("Barrel")]
    public BarrelTypes barrelType;
    [SerializeField] protected float bulletRange;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float bulletSizeMultiplier = 1f;
    [Header("Catalyst")]
    public CatalystTypes catalystType;
    [SerializeField] protected float damageModifier;
    [Header("Receiver")]
    [SerializeField] protected float fireRateModifier;
    [Header("Stock")]
    [SerializeField, Range(0f, 1f)] protected float recoilModifier;
    [SerializeField, Range(0f, 2f)] protected float movementModifier; 
    [Header("Scope")]
    [SerializeField, Range(0f, 1f)] protected float accuracyModifier;

    private void Awake()
    {
        gunController = FindAnyObjectByType<GunController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PickUp();
            gameObject.SetActive(false);
        }
    }

    private void PickUp()
    {
        gunController.AddAttachment(this);
    }
    public abstract void Attach();
    public abstract void DeAttach();
}

public enum AttachmentType
{
    Scope,
    Barrel,
    Magazine,
    Stock,
    Receiver,
    Catalyst
}

public enum MagazineType
{
    Normal,
    Explosion,
    Lightning,
    Fire,
}

public enum BarrelTypes
{
    Pistol,
    Shotgun,
    Rifle,
    Sniper
}

public enum CatalystTypes
{
    Laser,
    Bullet
}