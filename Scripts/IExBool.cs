namespace Moyba.Contracts
{
    /// <summary>
    /// Represents an eventable boolean value.
    /// </summary>
    public interface IExBool : IExValue<bool>
    {
        /// <summary>
        /// Occurs when the value changes to <c>false</c>.
        /// </summary>
        event ActionEventHandler OnFalse;

        /// <summary>
        /// Occurs when the value changes to <c>true</c>.
        /// </summary>
        event ActionEventHandler OnTrue;
    }

    /// <summary>
    /// Represents an eventable boolean value on an entity contract.
    /// </summary>
    /// <typeparam name="IEntity">The entity's type.</typeparam>
    public interface IExBool<out IEntity> : IExValue<IEntity, bool>
        where IEntity : class
    {
        /// <summary>
        /// Occurs when the value changes to <c>false</c>.
        /// </summary>
        event ActionEventHandler<IEntity> OnFalse;

        /// <summary>
        /// Occurs when the value changes to <c>true</c>.
        /// </summary>
        event ActionEventHandler<IEntity> OnTrue;
    }
}
