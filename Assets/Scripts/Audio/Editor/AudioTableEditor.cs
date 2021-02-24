using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(AudioTable))]
public class AudioTableEditor : Editor
{
    //private ReorderableList list;
    private AudioTable script;

    private ReorderableList _reorderableList;

    void OnEnable()
    {
        SerializedProperty prop = serializedObject.FindProperty("items");

        _reorderableList = new ReorderableList(serializedObject, prop);
        _reorderableList.elementHeight = 60;
        _reorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "List");
        _reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            SerializedProperty element = prop.GetArrayElementAtIndex(index);
            rect.height -= 4;
            rect.y += 0.125f;
            EditorGUI.PropertyField(rect, element);
        };
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();
        _reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

}
