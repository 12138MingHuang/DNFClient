public class HeroLogic : LogicActor
{
    /// <summary>
    /// 英雄ID
    /// </summary>
    public int HeroId { get; private set; }

    public HeroLogic(int heroId, RenderObject renderObject)
    {
        this.HeroId = heroId;
        this.RenderObject = renderObject;
        ObjectType = LogicObjectType.Hero;
    }
}