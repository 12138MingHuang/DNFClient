using FixMath;

public class FloatingBuff : BuffComposite
{

    public FloatingBuff(Buff buff) : base(buff) { }
    
    public override void BuffDelay()
    {
        
    }
    public override void BuffStart()
    {
        
    }
    public override void BuffTrigger()
    {
        if (buff.BuffConfig.buffParamsList.Count > 0)
        {
            FixInt floatingValue = buff.BuffConfig.buffParamsList[0].value;
            buff.attachTarget.AddRisingForce(floatingValue, buff.BuffConfig.buffDurationMS);
        }
    }
    public override void BuffEnd()
    {
        
    }
}