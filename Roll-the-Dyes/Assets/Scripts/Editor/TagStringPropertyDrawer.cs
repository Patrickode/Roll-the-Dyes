using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(TagString))]
public class TagStringPropertyDrawer : TagSelectorPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property.FindPropertyRelative("_value"), label);
    }
}