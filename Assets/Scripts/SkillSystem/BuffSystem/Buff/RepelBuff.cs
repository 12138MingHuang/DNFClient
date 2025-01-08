using FixMath;

/// <summary>
/// 击退Buff的实现类。
/// </summary>
public class RepelBuff : BuffComposite
{
    public RepelBuff(Buff buff) : base(buff) { }

    public override void BuffDelay() { }
    public override void BuffStart() { }
    public override void BuffTrigger()
    {
        if (buff.BuffConfig.buffParamsList.Count > 0)
        {
            // 获取击退距离
            FixInt repelValue = buff.BuffConfig.buffParamsList[0].value;
            FixIntVector3 endPos = new FixIntVector3(
                buff.releaser.LogicXAxis > 0 ? buff.attachTarget.LogicPos.x + repelValue : buff.attachTarget.LogicPos.x - repelValue,
                buff.attachTarget.LogicPos.y,
                buff.attachTarget.LogicPos.z
            );

            MoveToAction moveTo = new MoveToAction(buff.attachTarget, buff.attachTarget.LogicPos, 
                endPos, buff.BuffConfig.buffDurationMS, null, null, MoveType.X);
            
            LogicActionController.Instance.RunAction(moveTo);
        }
    }
    public override void BuffEnd() { }
}
