using FixMath;

public class GrabBuff : BuffComposite
{

    public GrabBuff(Buff buff) : base(buff) { }
    
    public override void BuffDelay()
    {
        
    }
    public override void BuffStart()
    {

    }
    public override void BuffTrigger()
    {
        // 更具抓取数据 让当前Buff的附加目标飞向指定位置
    }
    public override void BuffEnd()
    {
        // 1.计算飞向的目标点
        LogicObject attachTarget = buff.attachTarget;
        LogicObject releaser = buff.releaser;
        // 2.住区怪物至角色所在位置
        attachTarget.LogicPos = releaser.LogicPos;
        // 3.把怪物抓取到指定的目标点
        FixIntVector3 grabPos = new FixIntVector3(buff.BuffConfig.targetGrabData.garbMoveTargetPos) * releaser.LogicXAxis;
        grabPos.y = FixIntMath.Abs(grabPos.y);
        // 抓取目标位置
        FixIntVector3 targetGrabPos = releaser.LogicPos + grabPos;
        // 构建行动
        MoveToAction moveToAction = new MoveToAction(attachTarget, attachTarget.LogicPos, targetGrabPos,
            buff.BuffConfig.targetGrabData.moveTimeMS, null, null, MoveType.Target);
        // 执行行动
        LogicActionController.Instance.RunAction(moveToAction);
    }
}