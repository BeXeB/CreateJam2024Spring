using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{
    private Camera mainCamera;

    internal ObjectPool<Bullet> bulletPool;
    [SerializeField] private GameObject bullet;

    [SerializeField] internal List<Attachment> attachmentsInInventory = new();
    internal Scope equippedScope;
    internal Barrel equippedBarrel;
    internal Magazine equippedMagazine;
    internal Stock equippedStock;
    internal Receiver equippedReceiver;
    internal Catalyst equippedCatalyst;
    
    [SerializeField] private VisualEffect GunVFX;
    [SerializeField] private LayerMask raycastHitLayers;
    private Player playerController;
    
    internal Color vfxColor;
    internal MagazineType magazineType;
    [SerializeField] internal float bulletSpeed;
    [SerializeField] internal float fireRate;
    [SerializeField] internal float damage;
    [SerializeField, Range(0f, 1f)] internal float accuracy;
    [SerializeField, Range(0f, 1f)] internal float recoil;
    [SerializeField, Range(0f, 90f)] internal float maximumAccuracyAngle;
    
    private float nextFire;
    private float recoilBuildUp;

    private Vector3 mousePosition;
    private Vector3 shootingPosition;
    
    private bool isShooting;

    #region MonoBehaviour Methods
    private void Start()
    {
        mainCamera = Camera.main;
        
        playerController = GetComponent<Player>();

        playerController.playerControls.Player.Fire.performed += FireOnPerformed;
        playerController.playerControls.Player.Fire.canceled += FireOnCanceled;
        
        //Bullet pooling
        bulletPool = new ObjectPool<Bullet>(() => Instantiate(bullet).GetComponent<Bullet>(), GetBullet, ReleaseBullet, DestroyBullet, true, 50    , 1000);
        
        equippedScope = attachmentsInInventory.Find(defaultScope => defaultScope.name == "No Scope") as Scope;
        equippedBarrel = attachmentsInInventory.Find(defaultBarrel => defaultBarrel.name == "No Barrel") as Barrel;
        equippedMagazine = attachmentsInInventory.Find(defaultMagazine => defaultMagazine.name == "No Magazine") as Magazine;
        equippedStock = attachmentsInInventory.Find(defaultStock => defaultStock.name == "No Stock") as Stock;
        equippedReceiver = attachmentsInInventory.Find(defaultReceiver => defaultReceiver.name == "No Receiver") as Receiver;
        equippedCatalyst = attachmentsInInventory.Find(defaultCatalyst => defaultCatalyst.name == "No Catalyst") as Catalyst;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(playerController == null)
        {
            return;
        }
        
        float alpha = (accuracy + recoilBuildUp) / 2f * maximumAccuracyAngle;
        Vector3 position = playerController.transform.position;
        float x = Vector3.Distance(mousePosition, position) * Mathf.Sin(Mathf.Deg2Rad * (alpha / 2f));

        Gizmos.DrawWireSphere(mousePosition, x/2);
        
        Gizmos.DrawLine(position, mousePosition);
        Gizmos.DrawLine(position, shootingPosition);
    }
#endif
    
    private void OnDisable()
    {
        playerController.playerControls.Player.Fire.performed -= FireOnPerformed;
        playerController.playerControls.Player.Fire.canceled -= FireOnCanceled;
    }
    
    private void Update()
    {
        Physics.Raycast(mainCamera.ScreenToWorldPoint(playerController.playerControls.Player.MousePosition.ReadValue<Vector2>()), mainCamera.transform.forward, out RaycastHit cameraRay, 100f, raycastHitLayers);

        mousePosition = cameraRay.point;

        nextFire += Time.deltaTime;
        
        if(isShooting && nextFire >= 1f/fireRate)
        {
            if(recoilBuildUp < recoil)
            {
                recoilBuildUp += Time.deltaTime;
            }
            Vector3 position = playerController.transform.position;
            
            shootingPosition = CalculateRecoil(position);

            Vector3 shootDirection = shootingPosition - position;

            shootDirection.y = 0f;
            
            Bullet bulletInstance = bulletPool.Get();
            
            bulletInstance.bulletVFX.SetVector3("Direction", shootDirection.normalized);
            
            bulletInstance.rb.velocity = shootDirection.normalized * bulletSpeed;
            
            nextFire = 0f;
        }
        else if(!isShooting)
        {
            if(recoilBuildUp > 0f)
            {
                recoilBuildUp -= Time.deltaTime;
            }
            else
            {
                recoilBuildUp = 0f;
            }
        }
    }
    #endregion


    
    #region Bullet Pooling
    private void GetBullet(Bullet obj)
    {
        obj.gameObject.SetActive(true);
        
        obj.rb.position = playerController.transform.position;
    }
    
    private void ReleaseBullet(Bullet obj)
    {
        obj.rb.velocity = Vector3.zero;
        obj.gameObject.SetActive(false);
    }
    
    private void DestroyBullet(Bullet obj)
    {
        Destroy(obj.gameObject);
    }
    #endregion

    #region Inputs
    private void FireOnPerformed(InputAction.CallbackContext _)
    {
        Shoot();
    }

    private void FireOnCanceled(InputAction.CallbackContext _)
    {
        StopShooting();
    }
    #endregion

    public void AddAttachment(Attachment attachment)
    {
        attachmentsInInventory.Add(attachment);
    }

    public void EquipAttachment(Attachment attachment)
    {
        switch (attachment.attachmentType)
        {
            case AttachmentType.Scope:
                if(attachment != null && equippedScope != attachment)
                {
                    equippedScope = (Scope)attachment;
                    attachment.Attach();
                }
                break;
            case AttachmentType.Barrel:
                if(attachment != null && equippedBarrel != attachment)
                {
                    equippedBarrel = (Barrel)attachment;
                    attachment.Attach();
                }
                break;
            case AttachmentType.Magazine:
                if(attachment != null && equippedMagazine != attachment)
                {
                    equippedMagazine = (Magazine)attachment;
                    attachment.Attach();
                }
                break;
            case AttachmentType.Stock:
                if(attachment != null && equippedStock != attachment)
                {
                    equippedStock = (Stock)attachment;
                    attachment.Attach();
                }
                break;
            case AttachmentType.Receiver:
                if(attachment != null && equippedReceiver != attachment)
                {
                    equippedReceiver = (Receiver)attachment;
                    attachment.Attach();
                }
                break;
            case AttachmentType.Catalyst:
                if(attachment != null && equippedCatalyst != attachment)
                {
                    equippedCatalyst = (Catalyst)attachment;
                    attachment.Attach();
                }
                break;
        }
    }
    
    public void DeEquipAttachment(Attachment attachment)
    {
        switch (attachment.attachmentType)
        {
            case AttachmentType.Scope:
                if(equippedScope == attachment  && equippedScope.name != "No Scope")
                {
                    equippedScope = attachmentsInInventory.Find(defaultScope => defaultScope.name == "No Scope") as Scope;
                    attachment.DeAttach();
                }
                break;
            case AttachmentType.Barrel:
                if(equippedBarrel == attachment && equippedBarrel.name != "No Barrel")
                {
                    equippedBarrel = attachmentsInInventory.Find(defaultBarrel => defaultBarrel.name == "No Barrel") as Barrel;
                    attachment.DeAttach();
                }
                break;
            case AttachmentType.Magazine:
                if(equippedMagazine == attachment && equippedMagazine.name != "No Magazine")
                {
                    equippedMagazine = attachmentsInInventory.Find(defaultMagazine => defaultMagazine.name == "No Magazine") as Magazine;
                    attachment.DeAttach();
                }
                break;
            case AttachmentType.Stock:
                if(equippedStock == attachment && equippedStock.name != "No Stock")
                {
                    equippedStock = attachmentsInInventory.Find(defaultStock => defaultStock.name == "No Stock") as Stock;
                    attachment.DeAttach();
                }
                break;
            case AttachmentType.Receiver:
                if(equippedReceiver == attachment && equippedReceiver.name != "No Receiver")
                {
                    equippedReceiver = attachmentsInInventory.Find(defaultReceiver => defaultReceiver.name == "No Receiver") as Receiver;
                    attachment.DeAttach();
                }
                break;
            case AttachmentType.Catalyst:
                if(equippedCatalyst == attachment && equippedCatalyst.name != "No Catalyst")
                {
                    equippedCatalyst = attachmentsInInventory.Find(defaultCatalyst => defaultCatalyst.name == "No Catalyst") as Catalyst;
                    attachment.DeAttach();
                }
                break;
        }
    }
    
    private void Shoot()
    {
        isShooting = true;
    }
    
    private void StopShooting()
    {
        isShooting = false;
    }

    private Vector3 CalculateRecoil(Vector3 position)
    {
        float alpha = (accuracy + recoilBuildUp) / 2f * maximumAccuracyAngle;
        float x = Vector3.Distance(mousePosition, position) * Mathf.Sin(Mathf.Deg2Rad * (alpha / 2f));
        float r = x * Mathf.Sqrt(Random.Range(0f, 1f));
        float theta = Random.Range(0f, 1f) * 2 * Mathf.PI;
        float recoilZ = mousePosition.z + r * Mathf.Sin(theta);
        float recoilX = mousePosition.x + r * Mathf.Cos(theta);
        return new Vector3(recoilX, 0F, recoilZ);
    }
}
