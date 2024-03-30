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

    private void OnDisable()
    {
        if(returnToPoolCoroutine != null)
        {
            StopCoroutine(returnToPoolCoroutine);
        }
    }

    private void OnEnable()
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
        yield return new WaitForSeconds(5f);
        gunController.bulletPool.Release(this);
    }
}
