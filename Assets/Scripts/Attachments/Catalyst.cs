using System;

public class Catalyst : Attachment
{
    public override void Attach()
    {
        if(gunController == null)
        {
            gunController = FindAnyObjectByType<GunController>();
        }
        
        gunController.EquipAttachment(this);
        gunController.damage *= damageModifier;

        switch(catalystType)
        {
            case CatalystTypes.Laser:
                gunController.fireRate *= 25f;
                gunController.bulletSpeed *= 5f;
                gunController.accuracy *= 0.01f;
                gunController.recoil *= 0.01f;
                break;
        }
    }

    public override void DeAttach()
    {
        if(gunController == null)
        {
            gunController = FindAnyObjectByType<GunController>();
        }
        
        switch(catalystType)
        {
            case CatalystTypes.Laser:
                gunController.fireRate /= 25f;
                gunController.bulletSpeed /= 5f;
                accuracyModifier /= 0.01f;
                recoilModifier /= 0.01f;
                break;
        }
        
        gunController.DeEquipAttachment(this);
        gunController.damage /= damageModifier;
    }
}