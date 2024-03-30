public class Magazine : Attachment
{
    public override void Attach()
    {
        if(gunController == null)
        {
            gunController = FindAnyObjectByType<GunController>();
        }
        
        gunController.EquipAttachment(this);
        gunController.magazineType = magazineType;
        gunController.vfxColor = magazineColor;
    }

    public override void DeAttach()
    {
        if(gunController == null)
        {
            gunController = FindAnyObjectByType<GunController>();
        }
        
        gunController.DeEquipAttachment(this);
        gunController.magazineType = MagazineType.Normal;
        gunController.vfxColor = magazineColor;
    }
}