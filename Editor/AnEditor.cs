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

            var serializedProperty = this.serializedObject.GetIterator();
            if (serializedProperty.NextVisible(true))
            {
                do
                {
                    var propertyField = new PropertyField(serializedProperty.Copy())
                    {
                        name = $"PropertyField:{serializedProperty.propertyPath}"
                    };

                    if (serializedProperty.propertyPath.Equals("m_Script") && serializedObject.targetObject != null)
                    {
                        propertyField.SetEnabled(false);
                    }

                    inspectorGUI.Add(propertyField);
                }
                while (serializedProperty.NextVisible(false));
            }

            return inspectorGUI;
        }

        protected static Button _CreateButtonGUI(System.Action clickEvent, string text)
        {
            var button = new Button(clickEvent) { text = text };
            button.style.paddingLeft = 16;
            button.style.paddingRight = 16;
            button.style.paddingTop = 8;
            button.style.paddingBottom = 8;
            button.style.marginLeft = 4;
            button.style.marginRight = 4;
            return button;
        }

        protected static VisualElement _CreateHeaderGUI(string text)
        {
            var headerGUI = new VisualElement();
            headerGUI.AddToClassList("unity-decorator-drawers-container");

            var label = new Label(text);
            label.AddToClassList("unity-header-drawer__label");
            headerGUI.Add(label);

            return headerGUI;
        }

        protected static ObjectField _CreateObjectGUI(UnityEngine.Object value, string fieldName, bool isEnabled = false)
        {
            var name = ObjectNames.NicifyVariableName(fieldName);
            var objectField = new ObjectField(name);
            objectField.SetValueWithoutNotify(value);
            objectField.SetEnabled(isEnabled);
            objectField.Align();
            return objectField;
        }

        protected static Slider _CreateSliderGUI(
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

        protected static TextField _CreateTextFieldGUI(string value, string fieldName, bool isEnabled = false)
        {
            var name = ObjectNames.NicifyVariableName(fieldName);
            var textField = new TextField(name);
            textField.SetValueWithoutNotify(value);
            textField.SetEnabled(isEnabled);
            textField.Align();
            return textField;
        }

        protected static Toggle _CreateToggleGUI(bool value, string fieldName, bool isEnabled = false)
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
