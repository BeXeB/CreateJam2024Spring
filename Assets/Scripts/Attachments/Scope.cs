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
    }

    public override void DeAttach()
    {
        if(gunController == null)
        {
            gunController = FindAnyObjectByType<GunController>();
        }
        
        gunController.DeEquipAttachment(this);
        gunController.accuracy += accuracyModifier; 
    }
}