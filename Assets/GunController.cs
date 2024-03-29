using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{
    [SerializeField] private VisualEffect GunVFX;

    [SerializeField] private float fireRate;

    [SerializeField, Range(0f, 1f)] private float accuracy;
    [SerializeField, Range(0f, 1f)] private float recoil;
    [SerializeField, Range(0f, 90f)] private float maximumAcurracyAngle;
    
    private float recoilBuildUp;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDrawGizmos()
    {

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        float alpha = (accuracy + recoilBuildUp) / 2f * maximumAcurracyAngle;
        float x = Vector2.Distance(mousePosition, transform.position) * Mathf.Sin(Mathf.Deg2Rad * (alpha / 2f));

        Gizmos.DrawWireSphere(mousePosition, x/2);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(GunVFX.aliveParticleCount <= 0)
            {
                GunVFX.Play();
            }
            
            GunVFX.SetFloat("FireRate", fireRate);
        }
        
        if(Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;

            if(recoilBuildUp < recoil)
            {
                recoilBuildUp += Time.deltaTime;
            }

            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            
            //Recoil
            float alpha = (accuracy + recoilBuildUp) / 2f * maximumAcurracyAngle;
            float x = Vector2.Distance(mousePosition, transform.position) * Mathf.Sin(Mathf.Deg2Rad * (alpha / 2f));
            float r = x * Mathf.Sqrt(Random.Range(0f, 1f));
            float theta = Random.Range(0f, 1f) * 2 * Mathf.PI;
            float recoilY = mousePosition.y + r * Mathf.Sin(theta);
            float recoilX = mousePosition.x + r * Mathf.Cos(theta);
            
            mousePosition += new Vector3(recoilX, recoilY, 0f);

            Vector2 mouseDirection = mousePosition - transform.position;
            
            GunVFX.SetVector3("Direction", mouseDirection);
        }
        else
        {
            if(GunVFX.aliveParticleCount <= 0)
            {
                GunVFX.Stop();
            }
            
            if(recoilBuildUp > 0f)
            {
                recoilBuildUp -= Time.deltaTime;
            }
            else
            {
                recoilBuildUp = 0f;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            GunVFX.SetFloat("FireRate", 0f);
        }
        
    }
}
