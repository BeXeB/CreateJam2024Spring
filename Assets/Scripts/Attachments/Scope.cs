public class Scope : Attachment
{
    public override void Attach()
    {
        if(gunController == null)
        {
            gunController = FindAnyObjectByType<GunController>();
        }
        
        gunController.EquipAttachment(this);
        gunController.accuracy -= accuracyModifier;
        gunController.movementWhileShootingModifier = movementWhileShootingModifier;
        
        if(gunController.accuracy < 0)
        {
            gunController.accuracy = 0;
        }
    }

    public override void DeAttach()
    {
        if(gunController == null)
        {
            gunController = FindAnyObjectByType<GunController>();
        }
        
        gunController.DeEquipAttachment(this);
        gunController.accuracy += accuracyModifier; 
        
        if(gunController.accuracy > 1f)
        {
            gunController.accuracy = 1f;
        }
    }
}