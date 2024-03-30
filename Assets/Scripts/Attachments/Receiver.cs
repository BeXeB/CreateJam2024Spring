public class Receiver : Attachment
{
    public override void Attach()
    {
        gunController.EquipAttachment(this);
        gunController.fireRate += fireRateModifier;
    }

    public override void DeAttach()
    {
        gunController.DeEquipAttachment(this);
        gunController.fireRate -= fireRateModifier;
    }
}