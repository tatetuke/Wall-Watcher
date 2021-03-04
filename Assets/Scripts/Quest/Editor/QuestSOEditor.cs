using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

//https://qiita.com/OKsaiyowa/items/4a0c8c92271b6ad128c7
[CustomEditor(typeof(QuestDataSO))]
public class CustomFadeEditor : Editor
{
    ReorderableList reorderableList;
    ReorderableList reorderableList2;
    ReorderableList reorderableList3;

    void OnEnable()
    {
        SerializedProperty prop = serializedObject.FindProperty("subQuests");
        SerializedProperty prop2 = serializedObject.FindProperty("startConditions");
        SerializedProperty prop3 = serializedObject.FindProperty("endConditions");

        reorderableList = new ReorderableList(serializedObject, prop);
        reorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Sub Quests");
        reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            SerializedProperty element = prop.GetArrayElementAtIndex(index);
            rect.height -= 4;
            rect.y += 2;
            EditorGUI.PropertyField(rect, element);
        };

        reorderableList2 = new ReorderableList(serializedObject, prop2);
        reorderableList2.elementHeight = 68;
        reorderableList2.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "開始条件");
        reorderableList2.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            SerializedProperty element = prop2.GetArrayElementAtIndex(index);
            rect.height -= 4;
            rect.y += 2;
            EditorGUI.PropertyField(rect, element);
        };
        reorderableList3 = new ReorderableList(serializedObject, prop3);
        reorderableList3.elementHeight = 68;
        reorderableList3.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "終了条件");
        reorderableList3.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            SerializedProperty element = prop3.GetArrayElementAtIndex(index);
            rect.height -= 4;
            rect.y += 2;
            EditorGUI.PropertyField(rect, element);
        };
    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update();
        reorderableList.DoLayoutList();
        reorderableList2.DoLayoutList();
        reorderableList3.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}