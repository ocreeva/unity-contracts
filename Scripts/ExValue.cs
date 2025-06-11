using System.Collections.Generic;

namespace Moyba.Contracts
{
    public class ExValue<TValue> : IExValue<TValue>
    {
        private static readonly IEqualityComparer<TValue> _EqualityComparer = EqualityComparer<TValue>.Default;

        private TValue _value;

        public event ValueEventHandler<TValue> OnChanged;

        public TValue Value
        {
            get => _value;
            set
            {
                if (_EqualityComparer.Equals(_value, value)) return;

                _value = value;

                this.OnChanged?.Invoke(_value);
            }
        }
    }

    public class ExValue<IEntity, TValue> : IExValue<IEntity, TValue>
        where IEntity : class
    {
        private static readonly IEqualityComparer<TValue> _EqualityComparer = EqualityComparer<TValue>.Default;

        private readonly IEntity _entity;

        private TValue _value;

        public ExValue(IEntity entity) => _entity = entity;

        public event ValueEventHandler<IEntity, TValue> OnChanged;

        public TValue Value
        {
            get => _value;
            set
            {
                if (_EqualityComparer.Equals(_value, value)) return;

                _value = value;

                this.OnChanged?.Invoke(_entity, _value);
            }
        }
    }
}
