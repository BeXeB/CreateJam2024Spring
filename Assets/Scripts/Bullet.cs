using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    private GunController gunController;
    private Enemy homingTarget;
    [SerializeField] internal VisualEffect bulletVFX;
    internal Rigidbody rb;
    private Coroutine returnToPoolCoroutine;
    internal float timeToLiveModifier = 1f;
    
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
                break;
            case CatalystTypes.MagicSplitter:
                break;
            case CatalystTypes.Holy:
                break;
            case CatalystTypes.Infernal:
                break;
            case CatalystTypes.Homing:
                homingTarget = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
                    .OrderBy(enemy => Vector3.Distance(enemy.transform.position, transform.position)).FirstOrDefault();

                if(homingTarget != null)
                {
                    rb.velocity += (homingTarget.transform.position - transform.position).normalized * 0.1f;
                }
                break;
        }
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
