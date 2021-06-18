using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

//https://qiita.com/OKsaiyowa/items/4a0c8c92271b6ad128c7
[CustomEditor(typeof(QuestDataSO))]
public class CustomFadeEditor : Editor
{
    ReorderableList reorderableList;
    ReorderableList reorderableList3;
    QuestDataSO scr;
    void OnEnable()
    {
        scr = (QuestDataSO)target;
        SerializedProperty prop = serializedObject.FindProperty("subQuests");
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
        EditorGUILayout.Space();
        scr.title = EditorGUILayout.TextField("title",scr.title);
        EditorGUILayout.LabelField("description");
        scr.description= EditorGUILayout.TextArea(scr.description);
        EditorGUILayout.Space();
        serializedObject.Update();
        reorderableList.DoLayoutList();
        reorderableList3.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

        scr.flowchart = EditorGUILayout.ObjectField("flowchart", scr.flowchart, typeof(Fungus.Flowchart), true) as Fungus.Flowchart;
    }
}