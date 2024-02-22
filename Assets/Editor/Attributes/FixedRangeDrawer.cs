#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Utility.Attributes;

namespace Editor.Attributes
{
    /// <summary>
    /// Custom drawer for <see cref="FixedRangeAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(FixedRangeAttribute))]
    public class FixedRangeDrawer : PropertyDrawer
    {
        /// <summary>
        /// Get the height needed for a PropertyField control.
        /// </summary>
        /// <param name="property">The serialized property to make the custom GUI for.</param>
        /// <param name="label">The label of this property.</param>
        /// <returns>Property height.</returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        
        /// <summary>
        /// Overridden method to make own IMGUI based GUI for the property.
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the property GUI.</param>
        /// <param name="property">The serialized property to make the custom GUI for.</param>
        /// <param name="label">The label of this property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool prevGUIEnabled = GUI.enabled;

            if (EditorApplication.isPlaying) GUI.enabled = false;
            if (attribute is FixedRangeAttribute range)
            {
                switch (property.propertyType)
                {
                    case SerializedPropertyType.Float:
                        EditorGUI.Slider(position, property, range.Min, range.Max, label);
                        break;
                    case SerializedPropertyType.Integer:
                        EditorGUI.IntSlider(position, property, (int) range.Min, (int) range.Max, label);
                        break;
                    default:
                        Color previousColor = GUI.color;

                        GUI.color = Color.red;
                        EditorGUI.LabelField(position, label.text, "Use FixedRange with float or int.");
                        
                        GUI.color = previousColor;
                        break;
                }
            }

            GUI.enabled = prevGUIEnabled;
        }
    }
}
#endif