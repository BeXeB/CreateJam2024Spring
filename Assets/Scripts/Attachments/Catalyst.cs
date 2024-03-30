public class Catalyst : Attachment
{
    public override void Attach()
    {
        gunController.EquipAttachment(this);
        gunController.damage += damageModifier;
    }

    public override void DeAttach()
    {
        gunController.DeEquipAttachment(this);
        gunController.damage -= damageModifier;
    }
}