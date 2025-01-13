using FixMath;

public class AttributeModify_Buff_Single : BuffComposite
{
    public AttributeModify_Buff_Single(Buff buff) : base(buff) { }

    /// <summary>
    /// 配置的属性值增量
    /// </summary>
    private FixInt configValue;
    
    public override void BuffDelay()
    {

    }
    public override void BuffStart()
    {
        if(buff.BuffConfig.buffParamsList.Count > 0)
        {
            configValue = buff.BuffConfig.buffParamsList[0].value;
        }
    }
    public override void BuffTrigger()
    {
        ModifyAttribute(configValue);
    }
    
    /// <summary>
    /// 修改属性值
    /// </summary>
    /// <param name="value"> 增量 </param>
    private void ModifyAttribute(FixInt value)
    {
        switch (buff.BuffConfig.buffType)
        {
            case BuffType.MoveSpeed_Modify_Single:
                buff.attachTarget.LogicMoveSpeed += value;
                break;
        }
    }
    public override void BuffEnd()
    {
        ModifyAttribute(-configValue);
    }
}