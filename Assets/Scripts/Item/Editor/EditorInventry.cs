using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Kyoichi.Inventry))]
public class EditorInventry : Editor
{
    List<List<string>> editorCSVData = new List<List<string>>();
    //OnInspectorGUIでカスタマイズのGUIに変更する
    public override void OnInspectorGUI()
    {
        //元のInspector部分を表示する
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("inventry view", EditorStyles.centeredGreyMiniLabel);
        Kyoichi.Inventry scr = (Kyoichi.Inventry)target;
        if (scr.Data != null)
        {
            if (EditorApplication.isPlaying)
            {
                foreach (var i in scr.Data)
                {
                    string dataname;
                    if (i == null || i.item == null) dataname = "null";
                    else dataname = i.item.name;
                    i.count = EditorGUILayout.IntField(dataname, i.count);
                }
            }
            else//unityが実行されてないとき
            {
                if (editorCSVData != null)
                {
                    foreach (var i in editorCSVData)
                    {
                        if (i.Count < 2) continue;
                        EditorGUILayout.LabelField(i[0], i[1]);
                    }
                }
            }
        }
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        {
            //元のInspector部分の下にボタンを表示
            if (GUILayout.Button("Add item"))
            {
                scr.AddItem(Kyoichi.ItemManager.Instance.GetItem("error"));
            }
            GUI.color = Color.red;
            //元のInspector部分の下にボタンを表示
            if (GUILayout.Button("Clear item"))
            {
                scr.Clear();
            }
            GUI.color = Color.white;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        {
            if (GUILayout.Button("Load ItemSO"))
            {
                if (!EditorApplication.isPlaying)
                {
                    Debug.Log("ItemSO Loading");
                    Kyoichi.ItemManager.Instance.LoadAsync();
                }
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        {
            if (GUILayout.Button("Load from file"))
            {
                if (!EditorApplication.isPlaying)
                {
                    Debug.Log("Inventry loading");
                    scr.LoadFromFile();
                }
            }
            if (GUILayout.Button("Save to file"))
            {
                if (!EditorApplication.isPlaying)
                {
                    Debug.Log("Inventry saving");
                    scr.SaveToFile();
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}
