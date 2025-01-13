using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomPropertyDrawer(typeof(effects))]
public class EffectsDrawer : PropertyDrawer
{
    private GUIContent[] _options;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (_options == null)
        {
            _options = System.Enum.GetNames(typeof(effects))
                .Select(x => new GUIContent(x.Replace('_', '/')))
                .ToArray();
        }

        EditorGUI.BeginProperty(position, label, property);
        var current = property.enumValueIndex;
        var selected = EditorGUI.Popup(position, label, current, _options);
        property.enumValueIndex = selected;
        EditorGUI.EndProperty();
    }
}