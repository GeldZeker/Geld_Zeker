using UnityEditor;
using UnityEngine;

namespace TTimmermans.Utilities.ReadOnlyAttribute
{
    public class ReadOnlyAttribute : PropertyAttribute
    {
        /// <summary>A property Add-On to make fields Read-Only in the editor.</summary>
        [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
        public class ReadOnlyDrawer : PropertyDrawer
        {
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property, label, true);
                GUI.enabled = true;
            }
        }
    }
}