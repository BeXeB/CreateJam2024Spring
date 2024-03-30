using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{
    private ObjectPool<Rigidbody> bulletPool;
    
    [SerializeField] private GameObject bullet;
    [SerializeField] private VisualEffect GunVFX;
    [SerializeField] private LayerMask raycastHitLayers;
    private Player playerController;

    [SerializeField] private float fireRate;
    private float nextFire;

    [SerializeField, Range(0f, 1f)] private float accuracy;
    [SerializeField, Range(0f, 1f)] private float recoil;
    [SerializeField, Range(0f, 90f)] private float maximumAccuracyAngle;
    
    private Vector3 mousePosition;
    private Vector3 shootingPosition;
    
    private float recoilBuildUp;

    private bool isShooting;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        
        playerController = GetComponent<Player>();

        playerController.playerControls.Player.Fire.performed += FireOnPerformed;
        playerController.playerControls.Player.Fire.canceled += FireOnCanceled;
        
        //Bullet pooling
        bulletPool = new ObjectPool<Rigidbody>(() => Instantiate(bullet).GetComponent<Rigidbody>(), GetBullet, ReleaseBullet, DestroyBullet, false, 50    , 1000);
    }

    private void GetBullet(Rigidbody obj)
    {
        obj.gameObject.SetActive(true);
        obj.position = transform.position;
    }
    
    private void ReleaseBullet(Rigidbody obj)
    {
        obj.gameObject.SetActive(false);
    }
    
    private void DestroyBullet(Rigidbody obj)
    {
        Destroy(obj.gameObject);
    }

    private void OnDisable()
    {
        playerController.playerControls.Player.Fire.performed -= FireOnPerformed;
        playerController.playerControls.Player.Fire.canceled -= FireOnCanceled;
    }

    private void FireOnPerformed(InputAction.CallbackContext _)
    {
        Shoot();
    }

    private void FireOnCanceled(InputAction.CallbackContext _)
    {
        StopShooting();
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
    
    //Methods for buller pooling
    
    
    private void Shoot()
    {
        isShooting = true;
    }
    
    private void StopShooting()
    {
        isShooting = false;
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
            
            Rigidbody bulletInstance = bulletPool.Get();

            StartCoroutine(ReleaseBulletAfterTime(bulletInstance));
            
            bulletInstance.velocity = shootDirection.normalized * 50f;
            
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
    
    private IEnumerator ReleaseBulletAfterTime(Rigidbody bulletInstance)
    {
        yield return new WaitForSeconds(5f);
        bulletPool.Release(bulletInstance);
    }
}
