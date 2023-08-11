#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Utility.Attributes;

namespace Editor.Attributes
{
    /// <summary>
    /// Custom drawer for <see cref="RequireInterfaceAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(RequireInterfaceAttribute))]
    public class RequireInterfaceDrawer : PropertyDrawer
    {
        /// <summary>
        /// Overridden method to make own IMGUI based GUI for the property.
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the property GUI.</param>
        /// <param name="property">The serialized property to make the custom GUI for.</param>
        /// <param name="label">The label of this property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (attribute is RequireInterfaceAttribute requiredAttribute)
                {
                    EditorGUI.BeginProperty(position, label, property);
                    property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, requiredAttribute.RequiredType, true);
                    EditorGUI.EndProperty();
                }
                else
                {
                    Color previousColor = GUI.color;

                    GUI.color = Color.red;
                    EditorGUI.LabelField(position, label, new GUIContent($"Attribute is not of type {nameof(RequireInterfaceAttribute)}."));

                    GUI.color = previousColor;
                }
            }
            else
            {
                Color previousColor = GUI.color;
                
                GUI.color = Color.red;
                EditorGUI.LabelField(position, label, new GUIContent("Property is not a reference type."));
                
                GUI.color = previousColor;
            }
        }
    }
}
#endif