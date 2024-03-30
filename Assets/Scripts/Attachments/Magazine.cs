public class Magazine : Attachment
{
    public override void Attach()
    {
        gunController.EquipAttachment(this);
        gunController.magazineType = magazineType;
        gunController.vfxColor = magazineColor;
    }

    public override void DeAttach()
    {
        gunController.DeEquipAttachment(this);
        gunController.magazineType = MagazineType.Normal;
        gunController.vfxColor = magazineColor;
    }
}