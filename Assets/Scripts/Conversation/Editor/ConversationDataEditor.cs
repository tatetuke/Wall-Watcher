using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using RPGM.Gameplay;

[CustomEditor(typeof(ConversationData), true)]
public class AudioClipDataEditor : Editor
{
    ReorderableList list;
    ConversationData script;

    void OnEnable()
    {
        script = target as ConversationData;
        // list = new ReorderableList(serializedObject, serializedObject.FindProperty("items"), true, true, true, true);
        list = new ReorderableList(script.items, typeof(Conversations), true, true, true, true);
        list.drawElementCallback = OnDrawElement;
        //list.onAddCallback += OnAdd;
        list.onRemoveCallback += OnRemove;
        list.drawHeaderCallback += OnDrawHeader;
        list.onSelectCallback += OnSelect;
        list.onReorderCallback += OnReorder;
        Undo.undoRedoPerformed -= OnUndoRedo;
        Undo.undoRedoPerformed += OnUndoRedo;
        list.elementHeightCallback = index => 30;
        list.onAddDropdownCallback = (rect, list) =>
        {
            var menu = new GenericMenu();
            menu.AddItem(
                new GUIContent("Conversation"),
                false,
                () =>
                {
                    ConversationPieceWizard.New(script, ConversationType.normal);
                }
                );
            menu.AddItem(
                new GUIContent("Event"),
                false,
                () =>
                {
                    ConversationPieceWizard.New(script, ConversationType.events);
                }
                );
            menu.DropDown(rect);
        };
    }

    void OnSelect(ReorderableList list)
    {
    }
    void OnReorder(ReorderableList list)
    {
    }
    void OnDrawHeader(Rect rect)
    {
        GUI.Label(rect, "Conversation Script Items");
        if (list.list.Count != 0)
            script.m_firstConversation = ((Conversations)list.list[0]).id;
    }

    void OnUndoRedo()
    {
        if (serializedObject != null)
            serializedObject.Update();
    }


    /// <summary>
    /// reordableListで-ボタンを押したときの動作
    /// </summary>
    /// <param name="list"></param>
    void OnRemove(ReorderableList list)
    {
        var item = script.items[list.index];
        Undo.RecordObject(target, "Remove Item");
        script.Delete(item.id);
    }
    void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var item = (Conversations)list.list[index];
        var r = rect;
        switch (item.type)
        {
            case ConversationType.normal:
                GUI.color = Color.white;
                break;
            case ConversationType.events:
                GUI.color = new Color32(200, 160, 120,255);
                break;
            default:
                break;
        }
        r.width = rect.width * 0.2f;
        GUI.Label(r, item.id, EditorStyles.boldLabel);
        r.x += r.width;
        r.width = rect.width * 0.7f;
        switch (item.type)
        {
            case ConversationType.normal:
                GUI.Label(r, item.text);
                break;
            case ConversationType.events:
                GUI.Label(r, item.eventName);
                break;
            default:
                break;
        }

        r.x += r.width;
        r.width = rect.width * 0.1f;
        r.y -= 1;
        r.height -= 2;

        if (list.index == index)
        {
            if (GUI.Button(r, "Edit", EditorStyles.miniButton))
            {
                ConversationPieceWizard.Edit(script, item);
            }
        }
        r = rect;
        r.y += 15;
        GUI.Label(r, item.targetID);
        GUI.color = Color.white;
    }

    public override void OnInspectorGUI()
    {

        EditorGUILayout.PrefixLabel("Left Talker");
        //script.m_left = (TalkerData)EditorGUILayout.ObjectField(script.m_left, typeof(TalkerData), false);
        EditorGUILayout.PrefixLabel("Right Talker");
        //script.m_right = (TalkerData)EditorGUILayout.ObjectField(script.m_right, typeof(TalkerData), false);
        GUI.enabled = false;
        EditorGUILayout.TextField("FirstConversation", script.m_firstConversation);
        GUI.enabled = true;
        var questProperty = serializedObject.FindProperty("quest");
        if (questProperty != null)
            EditorGUILayout.PropertyField(questProperty, true);
        serializedObject.ApplyModifiedProperties();
        list.DoLayoutList();
    }
}
