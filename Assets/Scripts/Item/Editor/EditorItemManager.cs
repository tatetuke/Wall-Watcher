using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading;

[CustomEditor(typeof(Kyoichi.ItemManager))]
public class EditorItemManager : Editor
{

    private CancellationTokenSource _CancellationTokenSource;
    private void OnEnable()
    {
        _CancellationTokenSource = new CancellationTokenSource();
    }
    //OnInspectorGUIでカスタマイズのGUIに変更する
    public override void OnInspectorGUI()
    {
        //元のInspector部分を表示する
        base.OnInspectorGUI();
       var scr = (Kyoichi.ItemManager)target;
        EditorGUILayout.LabelField("loaded data view", EditorStyles.centeredGreyMiniLabel);
        foreach (var i in scr.Data)
        {
            EditorGUILayout.LabelField(i.Key);
        }
        //エディタ上でasync/awaitは使えないので無意味
        /*EditorGUILayout.BeginHorizontal(GUI.skin.box);
        {
            if (GUILayout.Button("Load from file"))
            {
                scr.Load(_CancellationTokenSource.Token);
            }
        }
        EditorGUILayout.EndHorizontal();*/

    }
}
