using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor.SceneManagement;

public class ConditionWizard : ScriptableWizard
{

    public string description;

        public QuestConditions conversationPiece, originalConversationPiece;
    public ReorderableList options;
    SerializedObject serializedObject;
    SerializedProperty descriptionProperty;
    SerializedProperty conditionProperty;
    bool isUpdate = false;
    string[] targets;

    public static void New(SerializedObject serializedObject,SerializedProperty descriptionProperty, SerializedProperty conditionProperty)
    {
        var w = ScriptableWizard.DisplayWizard<ConditionWizard>("New condition", "Create");
        w.description = "";
        w.descriptionProperty = descriptionProperty;
        w.conditionProperty = conditionProperty;
        w.serializedObject = serializedObject;
    }

    public static void Edit(SerializedObject serializedObject, SerializedProperty descriptionProperty, SerializedProperty conditionProperty)
    {
        var w = ScriptableWizard.DisplayWizard<ConditionWizard>("Edit condition", "Update");
        w.descriptionProperty = descriptionProperty;
        w.conditionProperty = conditionProperty;
        w.serializedObject = serializedObject;
    }


    void OnDrawOptionHeader(Rect rect)
    {
        GUI.Label(rect, "Branches");
    }

    void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = conditionProperty.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(rect, element);
    }

    private void OnAdd(ReorderableList list)
    {
        list.list.Add(new QuestConditions() { description = "" });
    }

    /// <summary>
    /// 右下にあるUpdateのボタンを押したときに実行される関数
    /// </summary>
    void OnWizardCreate()
    {
        // Undo.RecordObject(script, "Update Item");
     //   var script = (QuestConditions)serializedObject.targetObject;
      //  script.Set(originalConversationPiece, conversationPiece);
        // EditorUtility.SetDirty(script);
    }
    void BuildOptionList()
    {
        Debug.Log("Wizard created");
        options = new ReorderableList(serializedObject, conditionProperty);
        options.drawElementCallback = OnDrawElement;
        options.elementHeight = 68;
    }
    protected override bool DrawWizardGUI()
    {
        if (Event.current.isKey && Event.current.keyCode == KeyCode.Escape)
        {
            Close();
            return true;
        }
        EditorGUI.BeginChangeCheck();

        serializedObject.Update();
        // script.description = EditorGUILayout.TextArea(script.description);
       // var titleProp = serializedObject.FindProperty("title");
      //  EditorGUILayout.PropertyField(titleProp);
        EditorGUILayout.PropertyField(descriptionProperty);
        //des =EditorGUILayout.TextArea(des);
       // descriptionProp.stringValue = des;
        EditorGUILayout.Space(10);
        if (options == null) BuildOptionList();
        options.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

        return EditorGUI.EndChangeCheck();
    }


}
