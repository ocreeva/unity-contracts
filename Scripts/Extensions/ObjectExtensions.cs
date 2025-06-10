using UnityEngine;

public static class ObjectExtensions
{
    [System.Diagnostics.Conditional("UNITY_ASSERTIONS")]
    public static void _Assert(this Object source, bool condition, string message)
    => Debug.Assert(condition, $"{source.GetType().Name} on '{source.name}' {message}");

    [System.Diagnostics.Conditional("UNITY_ASSERTIONS")]
    public static void _Warn(this Object source, string message)
    => Debug.LogWarning($"{source.GetType().Name} on '{source.name}' {message}");
}
