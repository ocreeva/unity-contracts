using System;

namespace Moyba.Contracts
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class KeyAttribute : Attribute { }
}
