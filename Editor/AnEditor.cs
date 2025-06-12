using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Moyba.Contracts.Editor
{
    public abstract class AnEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var inspectorGUI = new VisualElement();

            return inspectorGUI;
        }

        protected VisualElement CreateHeaderGUI(string text)
        {
            var headerGUI = new VisualElement();
            headerGUI.AddToClassList("unity-decorator-drawers-container");

            var label = new Label(text);
            label.AddToClassList("unity-header-drawer__label");
            headerGUI.Add(label);

            return headerGUI;
        }

        protected PropertyField CreateSerializedProperty(string fieldName)
        => new PropertyField(this.serializedObject.FindProperty(fieldName));

        protected Slider CreateSliderGUI(
            float value,
            string fieldName,
            float start = 0,
            float end = 1,
            bool isEnabled = false)
        {
            var name = ObjectNames.NicifyVariableName(fieldName);
            var slider = new Slider(name, start, end);
            slider.SetValueWithoutNotify(value);
            slider.SetEnabled(isEnabled);
            slider.Align();
            return slider;
        }

        protected TextField CreateTextFieldGUI(string value, string fieldName, bool isEnabled = false)
        {
            var name = ObjectNames.NicifyVariableName(fieldName);
            var textField = new TextField(name);
            textField.SetValueWithoutNotify(value);
            textField.SetEnabled(isEnabled);
            textField.Align();
            return textField;
        }

        protected Toggle CreateToggleGUI(bool value, string fieldName, bool isEnabled = false)
        {
            var name = ObjectNames.NicifyVariableName(fieldName);
            var toggle = new Toggle(name);
            toggle.SetValueWithoutNotify(value);
            toggle.SetEnabled(isEnabled);
            toggle.Align();
            return toggle;
        }
    }
}
