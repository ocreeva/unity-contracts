namespace UnityEngine.UIElements
{
    public static class VisualElementExtensions
    {
        public static void Align(this VisualElement element)
        => element.AddToClassList("unity-base-field__aligned");
    }
}
