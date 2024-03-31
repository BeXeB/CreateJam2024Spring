using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    private GunController gunController;
    private GameObject homingTarget;
    [SerializeField] internal VisualEffect bulletVFX;
    internal Rigidbody rb;
    private Coroutine returnToPoolCoroutine;
    internal float timeToLiveModifier = 1f;

    internal bool isSplit;
    
    internal CatalystTypes catalystType;

    private void OnDisable()
    {
        if(returnToPoolCoroutine != null)
        {
            StopCoroutine(returnToPoolCoroutine);
        }
    }

    public void Instantiate()
    {
        catalystType = gunController.equippedCatalyst.catalystType;
        returnToPoolCoroutine = StartCoroutine(ReleaseBulletAfterTime());
    }

    private void Update()
    {
        switch(catalystType)
        {
            case CatalystTypes.Splitter:
                if(!isSplit)
                {
                    StartCoroutine(SplitBullet());
                }
                break;
            case CatalystTypes.MagicSplitter:
                if(!isSplit)
                {
                    StartCoroutine(MagicSplitBullet());
                }
                break;
            case CatalystTypes.Holy:
                break;
            case CatalystTypes.Infernal:
                break;
            case CatalystTypes.Homing: 
                List<GameObject> enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Select(enemy => enemy.transform.gameObject).ToList();           
                enemies.AddRange(FindObjectsByType<Dummy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Select(enemy => enemy.transform.gameObject).ToList());    
                if (enemies.Count > 0)
                {
                    homingTarget = enemies.OrderBy(enemy => (enemy.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
                }

                if(homingTarget != null)
                {
                    if(Vector3.Distance(homingTarget.transform.position, transform.position) < 5f)
                    {
                        rb.velocity = (homingTarget.transform.position - transform.position).normalized * gunController.bulletSpeed * 2f;
                    }
                    else
                    {
                        rb.velocity += (homingTarget.transform.position - transform.position).normalized * 0.5f;
                    }
                }
                break;
        }
    }

    private IEnumerator SplitBullet()
    {
        yield return new WaitForSeconds(1f);
        for(int i = -1; i < 2; i++)
        {
            Bullet bullet = gunController.bulletPool.Get();
            bullet.rb.position = rb.position;
            bullet.rb.velocity = Quaternion.Euler(0, 30 * i, 0) * rb.velocity;
            bullet.bulletVFX.SetVector3("Direction", bullet.rb.velocity.normalized);
            bullet.bulletVFX.SetVector4("Main Color", gunController.vfxColor);
            bullet.timeToLiveModifier = 0.5f;
            bullet.Instantiate();
            bullet.isSplit = true;
        }
        
        gunController.bulletPool.Release(this);
    }
    
    private IEnumerator MagicSplitBullet()
    {
        yield return new WaitForSeconds(1f);
        for(int i = -3; i < 4; i++)
        {
            Bullet bullet = gunController.bulletPool.Get();
            bullet.rb.position = rb.position;
            bullet.rb.velocity = Quaternion.Euler(0, 15 * i, 0) * rb.velocity;
            bullet.bulletVFX.SetVector3("Direction", bullet.rb.velocity.normalized);
            bullet.bulletVFX.SetVector4("Main Color", gunController.vfxColor);
            bullet.timeToLiveModifier = 0.25f;
            bullet.Instantiate();
            bullet.isSplit = true;
        }
        
        gunController.bulletPool.Release(this);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gunController = FindAnyObjectByType<GunController>();
    }

    public void ReturnToPool()
    {
        gunController.bulletPool.Release(this);
    }
    
    
    
    private IEnumerator ReleaseBulletAfterTime()
    {
        yield return new WaitForSeconds(1.5f * timeToLiveModifier);
        gunController.bulletPool.Release(this);
    }
}
