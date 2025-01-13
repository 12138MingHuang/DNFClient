public class IgnoreGravityBuff : BuffComposite
{
    public IgnoreGravityBuff(Buff buff) : base(buff) { }
    
    public override void BuffDelay()
    {

    }
    public override void BuffStart()
    {

    }
    public override void BuffTrigger()
    {
        buff.attachTarget.IsIgnoreGravity = true;
    }
    public override void BuffEnd()
    {
        buff.attachTarget.IsIgnoreGravity = false;
    }
}