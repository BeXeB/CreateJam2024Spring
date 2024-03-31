public class Receiver : Attachment
{
    public override void Attach()
    {
        if(gunController == null)
        {
            gunController = FindAnyObjectByType<GunController>();
        }
        
        gunController.EquipAttachment(this);
        gunController.fireRate = fireRate;
        
        switch(gunController.equippedCatalyst.catalystType)
        {
            case CatalystTypes.Laser:
                gunController.fireRate *= 25f;
                break;
        }

        if(name == "Minigun")
        {
            gunController.movementWhileShootingModifier += 0.33f;
        }
        else if(name == "Mythril Repeater")
        {
            gunController.movementWhileShootingModifier += 0.1f;
        }
    }

    public override void DeAttach()
    {
        if(gunController == null)
        {
            gunController = FindAnyObjectByType<GunController>();
        }
        
        if(name == "Minigun")
        {
            gunController.movementWhileShootingModifier -= 0.33f;
        }
        else if(name == "Mythril Repeater")
        {
            gunController.movementWhileShootingModifier -= 0.1f;
        }
        
        gunController.DeEquipAttachment(this);
    }
}
