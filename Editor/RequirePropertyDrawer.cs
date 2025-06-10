using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Moyba.Contracts.Editor
{
    [CustomPropertyDrawer(typeof(RequireAttribute))]
    public class RequirePropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var propertyGUI = property.propertyType == SerializedPropertyType.ObjectReference
                ? this.CreateTypedObjectGUI(property)
                : this.CreateErrorGUI();

            propertyGUI.Align();

            return propertyGUI;
        }

        private VisualElement CreateErrorGUI()
        {
            var errorGUI = new TextField(this.preferredLabel)
            {
                isReadOnly = true,
                value = "Property is not a reference type.",
            };
            errorGUI.labelElement.style.color = Color.red;

            return errorGUI;
        }

        private VisualElement CreateTypedObjectGUI(SerializedProperty property)
        {
            var requireAttribute = (RequireAttribute)this.attribute;

            return new ObjectField(this.preferredLabel)
            {
                bindingPath = property.propertyPath,
                objectType = requireAttribute.Type,
                value = property.objectReferenceValue,
            };
        }
    }
}
