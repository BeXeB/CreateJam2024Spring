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
    internal Player playerController;

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
    
    [SerializeField] internal Color vfxColor;
    internal MagazineType magazineType;
    [SerializeField] internal float bulletSpeed = 5f;
    [SerializeField] internal float fireRate = 5f;
    [SerializeField] internal float damage = 10;
    [SerializeField, Range(0f, 1f)] internal float accuracy = 1f;
    [SerializeField, Range(0f, 1f)] internal float recoil = 1f;
    [SerializeField, Range(0f, 90f)] internal float maximumAccuracyAngle = 45f;
    [SerializeField] internal float bulletRange;
    [SerializeField] internal float bulletSizeMultiplier = 1f;
    [SerializeField] internal float movementWhileShootingModifier = 1f;
    
    private float nextFire;
    private float recoilBuildUp;
    private float movementSpeedBuildUp;

    private Vector3 mousePosition;
    private Vector3 shootingPosition;
    
    private bool isShooting;
    private bool waitForClick;
    private bool isShotgun;


    #region MonoBehaviour Methods
    private void Start()
    {
        mainCamera = Camera.main;
        
        playerController = GetComponent<Player>();

        playerController.playerControls.Player.Fire.performed += FireOnPerformed;
        playerController.playerControls.Player.Fire.canceled += FireOnCanceled;
        
        //Bullet pooling
        bulletPool = new ObjectPool<Bullet>(() => Instantiate(bullet).GetComponent<Bullet>(), GetBullet, ReleaseBullet, DestroyBullet, true, 50    , 1000);
        
        equippedScope = attachmentsInInventory.Find(defaultScope => defaultScope.name == "Iron Sights") as Scope;
        equippedBarrel = attachmentsInInventory.Find(defaultBarrel => defaultBarrel.name == "Pistol Barrel") as Barrel;
        equippedMagazine = attachmentsInInventory.Find(defaultMagazine => defaultMagazine.name == "No Magazine") as Magazine;
        equippedStock = attachmentsInInventory.Find(defaultStock => defaultStock.name == "No Stock") as Stock;
        equippedReceiver = attachmentsInInventory.Find(defaultReceiver => defaultReceiver.name == "Semi-Automatic") as Receiver;
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

        if(nextFire < 1f / fireRate)
        {
            nextFire += Time.deltaTime;
        }
        
        if(equippedReceiver.receiverType is ReceiverType.SemiAuto or ReceiverType.Burst && isShooting && waitForClick && equippedCatalyst.catalystType != CatalystTypes.Laser)
        {
            return;
        }
        
        if(isShooting && nextFire >= 1f/fireRate)
        {
            if(recoilBuildUp < recoil)
            {
                recoilBuildUp += Time.deltaTime;
            }

            if(equippedReceiver.receiverType == ReceiverType.Burst)
            {
                StartCoroutine(ShootBurstBullets());
            }
            else
            {
                ShootBullet();
                
                nextFire = 0f;
            }
        }
        else if(!isShooting)
        {
            playerController.movementSpeedShootingReduction = 1f;

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
                if(equippedScope == attachment  && equippedScope.name != "Iron Sights")
                {
                    equippedScope = attachmentsInInventory.Find(defaultScope => defaultScope.name == "Iron Sights") as Scope;
                    attachment.DeAttach();
                }
                break;
            case AttachmentType.Barrel:
                if(equippedBarrel == attachment && equippedBarrel.name != "Pistol Barrel")
                {
                    equippedBarrel = attachmentsInInventory.Find(defaultBarrel => defaultBarrel.name == "Pistol Barrel") as Barrel;
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
                if(equippedReceiver == attachment && equippedReceiver.name != "Semi-Automatic")
                {
                    equippedReceiver = attachmentsInInventory.Find(defaultReceiver => defaultReceiver.name == "Semi-Automatic") as Receiver;
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
    
    private void ShootBullet()
    {
        playerController.Damage(10/fireRate);
        
        waitForClick = true;
        
        movementSpeedBuildUp = 1 - movementWhileShootingModifier * (recoilBuildUp / recoil);
            
        playerController.movementSpeedShootingReduction = movementSpeedBuildUp;
            
        Vector3 position = playerController.transform.position;
            
        shootingPosition = CalculateRecoil(position);

        Vector3 shootDirection = shootingPosition - position;

        shootDirection.y = 0f;

        if(!isShotgun)
        {
            Bullet bulletInstance = bulletPool.Get();
            
            bulletInstance.bulletVFX.SetVector3("Direction", shootDirection.normalized);
            
            bulletInstance.bulletVFX.SetVector4("Main Color", vfxColor);

            bulletInstance.timeToLiveModifier = bulletRange;
            
            bulletInstance.isSplit = false;
            
            bulletInstance.Instantiate();
            
            bulletInstance.transform.localScale = bullet.transform.localScale * bulletSizeMultiplier;
            
            bulletInstance.rb.velocity = shootDirection.normalized * bulletSpeed;
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
                Bullet bulletInstance = bulletPool.Get();
            
                bulletInstance.bulletVFX.SetVector3("Direction", shootDirection.normalized);
            
                bulletInstance.bulletVFX.SetVector4("Main Color", vfxColor);

                bulletInstance.timeToLiveModifier = bulletRange;
            
                bulletInstance.Instantiate();

                bulletInstance.isSplit = false;
            
                bulletInstance.transform.localScale = bullet.transform.localScale * bulletSizeMultiplier;
            
                shootDirection = shootingPosition + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f)) - position;
                    
                shootDirection.y = 0f;
                    
                bulletInstance.rb.velocity = shootDirection.normalized * bulletSpeed;
            }
        }
        AudioMananger.instance.PlayAudioClip("Shooting");
    }
    
    private void Shoot()
    {
        isShooting = true;
        waitForClick = false;
    }
    
    private void StopShooting()
    {
        isShooting = false;
    }
    
    private IEnumerator ShootBurstBullets()
    {
        for (int i = 0; i < 3; i++)
        {
            nextFire = 0f;
            
            ShootBullet();

            yield return new WaitForSeconds(1f/fireRate/3f);
            
            nextFire = 0f;
        }
        
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

    public void EquipShotgun()
    {
        isShotgun = true;
        damage *= 0.2f;
        fireRate *= 0.25f;
    }

    public void RemoveShotgun()
    {
        isShotgun = false;
        damage *= 5f;
        fireRate *= 4f;
    }
}
