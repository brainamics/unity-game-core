using UnityEditor;
using UnityEngine;

namespace Brainamics.Core
{
    [CustomPropertyDrawer(typeof(CellAddress))]
    public class CellAddressEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            // Get properties
            var rowProp = property.FindPropertyRelative("Row");
            var columnProp = property.FindPropertyRelative("Column");
            
            // Calculate rects for layout
            var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
            var fieldWidth = (position.width - EditorGUIUtility.labelWidth) / 2;
            var padding = 5f;
            
            var rowLabelRect = new Rect(
                labelRect.x + EditorGUIUtility.labelWidth, 
                position.y, 
                30, 
                position.height);
                
            var rowFieldRect = new Rect(
                rowLabelRect.x + rowLabelRect.width, 
                position.y, 
                fieldWidth - rowLabelRect.width - padding, 
                position.height);
                
            var colLabelRect = new Rect(
                rowFieldRect.x + rowFieldRect.width + padding, 
                position.y, 
                30, 
                position.height);
                
            var colFieldRect = new Rect(
                colLabelRect.x + colLabelRect.width, 
                position.y,
                fieldWidth - colLabelRect.width - padding, 
                position.height);
                
            // Draw main label
            EditorGUI.LabelField(labelRect, label);
            
            // Draw row and column labels and fields
            EditorGUI.LabelField(rowLabelRect, "Row");
            EditorGUI.PropertyField(rowFieldRect, rowProp, GUIContent.none);
            
            EditorGUI.LabelField(colLabelRect, "Col");
            EditorGUI.PropertyField(colFieldRect, columnProp, GUIContent.none);
            
            EditorGUI.EndProperty();
        }
    }
}