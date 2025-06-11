namespace Moyba.Contracts
{
    public delegate void ActionEventHandler();
    public delegate void ActionEventHandler<in IEntity>(IEntity entity)
        where IEntity : class;
}
