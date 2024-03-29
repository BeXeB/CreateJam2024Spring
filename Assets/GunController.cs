using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunController : MonoBehaviour
{
    [SerializeField] private VisualEffect GunVFX;
    // Start is called before the first frame update
    void Start()
    {
        
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
            
            GunVFX.SetFloat("FireRate", 1f);
        }
        
        if(Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;

            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector2 mouseDirection = mousePosition - transform.position;
            
            GunVFX.SetVector3("Direction", mouseDirection);
        }

        if(Input.GetMouseButtonUp(0))
        {
            GunVFX.SetFloat("FireRate", 0f);
        }
        
    }
}
