public abstract class BuffComposite
{
    protected Buff buff;
    
    public BuffComposite(Buff buff)
    {
        this.buff = buff;
    }

    /// <summary>
    /// buff延迟触发接口
    /// </summary>
    public abstract void BuffDelay();

    /// <summary>
    /// buff开始触发接口
    /// </summary>
    public abstract void BuffStart();

    /// <summary>
    /// buff逻辑触发，可以执行晕眩逻辑或属性修改逻辑
    /// </summary>
    public abstract void BuffTrigger();

    /// <summary>
    /// buff执行完成
    /// </summary>
    public abstract void BuffEnd();
}