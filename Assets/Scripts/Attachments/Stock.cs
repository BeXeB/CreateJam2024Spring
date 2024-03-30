public class Stock : Attachment
{
    public override void Attach()
    {
        gunController.EquipAttachment(this);
        gunController.recoil -= recoilModifier;
    }

    public override void DeAttach()
    {
        gunController.DeEquipAttachment(this);
        gunController.recoil += recoilModifier;
    }
}