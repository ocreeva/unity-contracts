namespace Moyba.Contracts
{
    public class ExBool : ExValue<bool>, IExBool
    {
        public ExBool() => this.OnChanged += this.HandleChanged;

        public event ActionEventHandler OnFalse;
        public event ActionEventHandler OnTrue;

        private void HandleChanged(bool value)
        => (value ? this.OnTrue : this.OnFalse)?.Invoke();
    }

    public class ExBool<IEntity> : ExValue<IEntity, bool>, IExBool<IEntity>
        where IEntity : class
    {
        public ExBool(IEntity entity) : base(entity) => this.OnChanged += this.HandleChanged;

        public event ActionEventHandler<IEntity> OnFalse;
        public event ActionEventHandler<IEntity> OnTrue;

        private void HandleChanged(IEntity entity, bool value)
        => (value ? this.OnTrue : this.OnFalse)?.Invoke(entity);
    }
}
