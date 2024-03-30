using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    private GunController gunController;
    [SerializeField] internal VisualEffect bulletVFX;
    internal Rigidbody rb;
    private Coroutine returnToPoolCoroutine;
    internal float timeToLiveModifier = 1f;

    private void OnDisable()
    {
        if(returnToPoolCoroutine != null)
        {
            StopCoroutine(returnToPoolCoroutine);
        }
    }

    public void Instantiate()
    {
        returnToPoolCoroutine = StartCoroutine(ReleaseBulletAfterTime());
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
