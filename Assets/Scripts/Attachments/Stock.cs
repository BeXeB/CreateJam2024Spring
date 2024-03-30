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
    }
}