public class Barrel : Attachment
{
    public override void Attach()
    {
        if(gunController == null)
        {
            gunController = FindAnyObjectByType<GunController>();
        }
        
        gunController.EquipAttachment(this);
        gunController.bulletSpeed = bulletSpeed;
        gunController.bulletRange = bulletRange;
        gunController.bulletSizeMultiplier = bulletSizeMultiplier;
        if(barrelType == BarrelTypes.Shotgun)
        {
            gunController.EquipShotgun();
        }
        
        switch(gunController.equippedCatalyst.catalystType)
        {
            case CatalystTypes.Laser:
                gunController.bulletSpeed *= 5f;
                break;
        }
    }

    public override void DeAttach()
    {
        if(gunController == null)
        {
            gunController = FindAnyObjectByType<GunController>();
        }
        
        gunController.DeEquipAttachment(this);

        if(barrelType == BarrelTypes.Shotgun)
        {
            gunController.RemoveShotgun();
        }
    }
}