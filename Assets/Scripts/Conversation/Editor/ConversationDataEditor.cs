using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using RPGM.Gameplay;

[CustomEditor(typeof(ConversationData), true)]
public class AudioClipDataEditor : Editor
{
    ReorderableList m_list;
    ConversationData m_script;

    void OnEnable()
    {
        m_script = target as ConversationData;
        // list = new ReorderableList(serializedObject, serializedObject.FindProperty("items"), true, true, true, true);
        m_list = new ReorderableList(m_script.items, typeof(Conversations), true, true, true, true);
        m_list.drawElementCallback = OnDrawElement;
        //list.onAddCallback += OnAdd;
        m_list.onRemoveCallback += OnRemove;
        m_list.drawHeaderCallback += OnDrawHeader;
        m_list.onSelectCallback += OnSelect;
        m_list.onReorderCallback += OnReorder;
        Undo.undoRedoPerformed -= OnUndoRedo;
        Undo.undoRedoPerformed += OnUndoRedo;
        m_list.elementHeightCallback = index => 30;
        m_list.onAddDropdownCallback = (rect, list) =>
        {
            var menu = new GenericMenu();
            menu.AddItem(
                new GUIContent("Conversation"),
                false,
                () =>
                {
                    ConversationPieceWizard.New(m_script, ConversationType.normal);
                }
                );
            menu.AddItem(
                new GUIContent("Event"),
                false,
                () =>
                {
                    ConversationPieceWizard.New(m_script, ConversationType.events);
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
        if (m_list.list.Count != 0)
            m_script.m_firstConversation = ((Conversations)m_list.list[0]).id;
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
        var item = m_script.items[list.index];
        Undo.RecordObject(target, "Remove Item");
        m_script.Delete(item.id);
    }
    void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var item = (Conversations)m_list.list[index];
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

        if (m_list.index == index)
        {
            if (GUI.Button(r, "Edit", EditorStyles.miniButton))
            {
                ConversationPieceWizard.Edit(m_script, item);
            }
        }
        r = rect;
        r.y += 15;
        GUI.Label(r, item.targetID);
        GUI.color = Color.white;
    }

    public override void OnInspectorGUI()
    {

       // EditorGUILayout.PrefixLabel("Left Talker");
        //script.m_left = (TalkerData)EditorGUILayout.ObjectField(script.m_left, typeof(TalkerData), false);
       // EditorGUILayout.PrefixLabel("Right Talker");
        //script.m_right = (TalkerData)EditorGUILayout.ObjectField(script.m_right, typeof(TalkerData), false);
        GUI.enabled = false;
        EditorGUILayout.TextField("FirstConversation", m_script.m_firstConversation);
        GUI.enabled = true;
        var questProperty = serializedObject.FindProperty("quest");
        if (questProperty != null)
            EditorGUILayout.PropertyField(questProperty, true);
        serializedObject.ApplyModifiedProperties();
        m_list.DoLayoutList();
    }
}
