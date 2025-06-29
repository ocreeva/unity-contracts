namespace Moyba.Contracts
{
    /// <summary>
    /// Represents an eventable value.
    /// </summary>
    /// <typeparam name="TValue">The value's type.</typeparam>
    public interface IExValue<out TValue> : IValue<TValue>
    {
        /// <summary>
        /// Occurs after the value changed.
        /// </summary>
        event ValueEventHandler<TValue> OnChanged;

        /// <summary>
        /// Occurs before the value changes.
        /// </summary>
        event ValueEventHandler<TValue> OnChanging;
    }

    /// <summary>
    /// Represents an eventable value on an entity contract.
    /// </summary>
    /// <typeparam name="IEntity">The entity's type.</typeparam>
    /// <typeparam name="TValue">The value's type.</typeparam>
    public interface IExValue<out IEntity, out TValue> : IValue<TValue>
        where IEntity : class
    {
        /// <summary>
        /// Occurs after the value changed.
        /// </summary>
        event ValueEventHandler<IEntity, TValue> OnChanged;

        /// <summary>
        /// Occurs before the value changes.
        /// </summary>
        event ValueEventHandler<IEntity, TValue> OnChanging;
    }
}
