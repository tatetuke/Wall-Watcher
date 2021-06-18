using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Playables;
using UnityEditor.SceneManagement;
using RPGM.Gameplay;

namespace RPGM.Gameplay
{

    public class ConversationPieceWizard : ScriptableWizard
    {

        public Conversations conversationPiece, originalConversationPiece;

        public ReorderableList options;
        public ReorderableList conditions;
        SerializedProperty list_prop;
        ConversationData conversationScript;
        string[] targets;
        int conversation_index;

        public static void Edit(SerializedProperty list_prop, ConversationData conversationScript, Conversations conversationPiece, int index)
        {
            var w = ScriptableWizard.DisplayWizard<ConversationPieceWizard>("Edit Conversation Piece", "Update");
            w.targets = (from i in conversationScript.items select i.id).ToArray();
            w.originalConversationPiece = conversationPiece;
            w.conversationPiece = conversationPiece;
            w.conversationScript = conversationScript;
            w.list_prop = list_prop;
            w.conversation_index = index;
        }

        void OnDrawOption(Rect rect, int index, bool isActive, bool isFocused)
        {
            var item = conversationPiece.options[index];
            var i = System.Array.IndexOf(targets, item.targetId);
            if (i < 0) i = 0;
            var popupRect = rect;
            popupRect.height = 16;
            popupRect.width = rect.width * 0.2f;
            i = EditorGUI.Popup(popupRect, i, targets);//targetID候補をドロップダウンで表示
            item.targetId = targets[i];
            //r.x += r.width;
            // item.image = (Sprite)EditorGUI.ObjectField(r, item.image, typeof(Sprite), false);
            var textRect = rect;
            textRect.x += popupRect.width + rect.width * 0.2f;
            textRect.width = rect.width * 0.6f;
            item.text = EditorGUI.TextField(textRect, item.text);
            conversationPiece.options[index] = item;
        }

        void OnDrawCondition(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty piece = list_prop.GetArrayElementAtIndex(conversation_index);
            SerializedProperty prop = piece.FindPropertyRelative("conditions");
            if (prop.arraySize >= index)
            {
                Debug.Log($"array size out of range {prop.arraySize}");
                return;
            }
            SerializedProperty element = prop.GetArrayElementAtIndex(index);
            if (element == null)
            {
                return;
            }
            EditorGUI.PropertyField(rect, element);
        }

        /// <summary>
        /// 右下にあるUpdateのボタンを押したときに実行される関数
        /// </summary>
        void OnWizardCreate()
        {
            Undo.RecordObject(conversationScript, "Update Item");
            conversationScript.Set(originalConversationPiece, conversationPiece);
            EditorUtility.SetDirty(conversationScript);
        }

        protected override bool DrawWizardGUI()
        {
            if (Event.current.isKey && Event.current.keyCode == KeyCode.Escape)//エスケープキー押したらウィンドウを閉じる
            {
                Close();
                return true;
            }
            isValid = true;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PrefixLabel("ID");
            conversationPiece.id = EditorGUILayout.TextField(conversationPiece.id).Trim().ToUpper();
            if (string.IsNullOrEmpty(conversationPiece.id))//IDが空白ならエラー
            {
                EditorGUILayout.HelpBox("The ID field cannot be empty.", MessageType.Error);
                isValid = false;
            }
            else if (conversationPiece.id != originalConversationPiece.id)//元のIDと違う（編集された）とき
            {
                if (conversationScript.ContainsKey(conversationPiece.id))
                {
                    EditorGUILayout.HelpBox("This ID already exists in this conversation and cannot be saved.", MessageType.Error);
                    isValid = false;
                }
                else
                {
                    EditorGUILayout.HelpBox("ID has changed and will be updated in related records", MessageType.Warning);
                }
            }

            EditorGUILayout.PrefixLabel("TargetID");
            conversationPiece.targetID = EditorGUILayout.TextField(conversationPiece.targetID).Trim().ToUpper();
            if (conversationPiece.targetID != originalConversationPiece.targetID)//元のIDと違う（編集された）とき
            {
                if (conversationPiece.targetID == conversationPiece.id)
                {
                    EditorGUILayout.HelpBox("This ID targets own conversation.", MessageType.Error);
                    isValid = false;
                }
                else
                {
                    EditorGUILayout.HelpBox("ID has changed and will be updated in related records", MessageType.Warning);
                }
            }

            DrawNormalGUI();

            return EditorGUI.EndChangeCheck();
        }

        void DrawNormalGUI()
        {
            EditorGUILayout.PrefixLabel("Text");
            conversationPiece.text = EditorGUILayout.TextArea(conversationPiece.text);
            EditorGUILayout.PrefixLabel("Timeline");
            conversationPiece.playableDirector = (PlayableDirector)EditorGUILayout.ObjectField(conversationPiece.playableDirector, typeof(PlayableDirector), false);
            EditorGUILayout.PrefixLabel("Audio");
            conversationPiece.audio = (AudioClip)EditorGUILayout.ObjectField(conversationPiece.audio, typeof(AudioClip), false);
            EditorGUILayout.PrefixLabel("Quest (Optional)");
            conversationPiece.quest = (QuestDataSO)EditorGUILayout.ObjectField(conversationPiece.quest, typeof(QuestDataSO), true);

            /*SerializedProperty piece = list_prop.GetArrayElementAtIndex(conversation_index);
            SerializedProperty prop = piece.FindPropertyRelative("conditions");
            if (conditions == null)
            {
                conditions = new ReorderableList(conversationPiece.conditions, typeof(QuestConditions), true, true, true, true);
                conditions.elementHeight = 68;
                conditions.onAddCallback = (list) =>
                {
                    conversationScript.items[conversation_index].conditions.Add(new QuestConditions());
                };
               conditions.drawElementCallback = OnDrawCondition;
            }
            conditions.DoLayoutList();*/
            if (options == null)
            {
                options = new ReorderableList(conversationPiece.options, typeof(ConversationOption), true, true, true, true);
                options.drawElementCallback = OnDrawOption;
                options.drawHeaderCallback = (rect) =>
                {
                    GUI.Label(rect, "Branches");
                };
            }
            options.DoLayoutList();
        }
    }
}

/* void DrawEventGUI()
 {
     EditorGUILayout.PrefixLabel("Event Name");
     conversationPiece.eventName = EditorGUILayout.TextArea(conversationPiece.eventName);
 }*/