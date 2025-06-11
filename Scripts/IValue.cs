namespace Moyba.Contracts
{
    /// <summary>
    /// Represents a value.
    /// </summary>
    /// <typeparam name="TValue">The value's type.</typeparam>
    public interface IValue<out TValue>
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        TValue Value { get; }
    }
}
