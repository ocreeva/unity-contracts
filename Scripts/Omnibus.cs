using UnityEngine;

namespace Moyba.Contracts
{
    [DefaultExecutionOrder(-99)]
    public partial class Omnibus : AContract
    {
        private static readonly System.Action<Omnibus> _AssignManagerProperties;
#if UNITY_EDITOR
        private static readonly System.Action<Omnibus> _ResetManagerFields;
#endif

        private static Omnibus _Instance;

        static Omnibus()
        {
            _AssignManagerProperties = _Reflection_AssignManagerProperties();
#if UNITY_EDITOR
            _ResetManagerFields = _Reflection_ResetManagerFields();
#endif
        }

        private void Awake()
        {
            if (_Instance)
            {
                if (!ReferenceEquals(_Instance, this)) Object.Destroy(this.gameObject);
                return;
            }

            _Instance = this;
            Object.DontDestroyOnLoad(this.gameObject);

            _AssignManagerProperties(this);
        }

#if UNITY_EDITOR
        private void Reset()
        => _ResetManagerFields(this);
#endif
    }
}
