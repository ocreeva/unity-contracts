namespace Moyba.Contracts
{
    public delegate void ActionEventHandler<in TEntity>(TEntity entity)
        where TEntity : class;
}
