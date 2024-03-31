public class Stock : Attachment
{
    public override void Attach()
    {
        if(gunController == null)
        {
            gunController = FindAnyObjectByType<GunController>();
        }
        
        gunController.EquipAttachment(this);
        gunController.recoil -= recoilModifier;
        gunController.playerController.movementSpeed *= movementModifier;
        
        if(gunController.recoil < 0)
        {
            gunController.recoil = 0;
        }
    }

    public override void DeAttach()
    {
        if(gunController == null)
        {
            gunController = FindAnyObjectByType<GunController>();
        }
        
        gunController.DeEquipAttachment(this);
        gunController.recoil += recoilModifier;
        gunController.playerController.movementSpeed /= movementModifier;
        
        if(gunController.recoil > 1f)
        {
            gunController.recoil = 1f;
        }
    }
}