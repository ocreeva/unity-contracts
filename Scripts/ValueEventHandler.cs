namespace Moyba.Contracts
{
    public delegate void ValueEventHandler<in TValue>(TValue value);
    public delegate void ValueEventHandler<in IEntity, in TValue>(IEntity entity, TValue value)
        where IEntity : class;
}
