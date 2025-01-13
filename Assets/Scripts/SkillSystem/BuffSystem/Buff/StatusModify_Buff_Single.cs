using FixMath;

public class StatusModify_Buff_Single : BuffComposite
{
    public StatusModify_Buff_Single(Buff buff) : base(buff) { }
    
    public override void BuffDelay()
    {

    }
    public override void BuffStart()
    {

    }
    public override void BuffTrigger()
    {
        ModifyStatus(true);
    }
    private void ModifyStatus(bool value)
    {
        switch (buff.BuffConfig.buffType)
        {
            case BuffType.AllowMove:
                buff.attachTarget.IsForceAllowMove = value;
                break;
            case BuffType.NotAllowDir:
                buff.attachTarget.IsForceNotAllowModifyDir = value;
                break;
        }
    }

    public override void BuffEnd()
    {
        ModifyStatus(false);
    }
}
