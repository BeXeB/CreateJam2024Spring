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
                List<GameObject> enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Select(enemy => enemy.transform.gameObject).ToList();           
                enemies.AddRange(FindObjectsByType<Dummy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Select(enemy => enemy.transform.gameObject).ToList());    
                if (enemies.Count > 0)
                {
                    // Get the closest enemy to the bullet
                    Debug.Log(1);

                    homingTarget = enemies.OrderBy(enemy => (enemy.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
                }
                //get closest enemy to bullet
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
