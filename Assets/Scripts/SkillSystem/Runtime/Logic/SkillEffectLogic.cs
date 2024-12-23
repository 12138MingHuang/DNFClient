using FixMath;

public class SkillEffectLogic : LogicObject
{
    /// <summary>
    /// 技能创建者
    /// </summary>
    private LogicActor mSkillCreator;
    /// <summary>
    /// 技能特效配置
    /// </summary>
    private SkillEffectConfig mEffectcfg;

    public SkillEffectLogic(LogicObjectType objType, SkillEffectConfig effectCfg, RenderObject renderObject, LogicActor skillCreator)
    {
        ObjectType = objType;
        mEffectcfg = effectCfg;
        mSkillCreator = skillCreator;
        RenderObject = renderObject;
        LogicXAxis = skillCreator.LogicXAxis;
        //初始化特效逻辑位置
        if (effectCfg.effectPosType == EffectPosType.FollowDir || effectCfg.effectPosType == EffectPosType.FollowPosDir)
        {
            FixIntVector3 offsetPos = new FixIntVector3(effectCfg.effectOffsetPos) * LogicXAxis;
            offsetPos.y = FixIntMath.Abs(offsetPos.y);
            LogicPos = skillCreator.LogicPos + offsetPos;
        }
    }
}
