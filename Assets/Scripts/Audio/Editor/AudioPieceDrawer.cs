using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AddressableAssets;


[CustomPropertyDrawer(typeof(AudioPiece))]
public class AudioPieceDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //元は 1 つのプロパティーであることを示すために PropertyScope で囲む
        using (new EditorGUI.PropertyScope(position, label, property))
        {
            //ラベル領域の幅を調整
            EditorGUIUtility.labelWidth = 65;

            position.height = EditorGUIUtility.singleLineHeight;

            //各プロパティーの Rect を求める
            Rect idRect = new Rect(position)
            {
                y = position.y + EditorGUIUtility.singleLineHeight + 1
            };

            Rect audioRect = new Rect(idRect)
            {
                y = idRect.y + EditorGUIUtility.singleLineHeight + 1
            };

            //各プロパティーの SerializedProperty を求める
            SerializedProperty idProperty = property.FindPropertyRelative("id");
            SerializedProperty referenceProperty = property.FindPropertyRelative("reference");

            //各プロパティーの GUI を描画
            //using (new EditorGUILayout.HorizontalScope())
            //{
            //    idProperty.stringValue = EditorGUILayout.TextField("id", idProperty.stringValue);
            //    EditorGUILayout.ObjectField(audioClipProperty);
            //}
            idProperty.stringValue = EditorGUI.TextField(idRect, "id", idProperty.stringValue);
            EditorGUI.PropertyField(audioRect, referenceProperty);

        }
    }
}