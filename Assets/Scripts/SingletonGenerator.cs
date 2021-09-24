using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DontDestroyOnLoadでほかのシーンから流れてきたシングルトンと競合しないようにする
/// シングルトンがすでにあれば生成せず、なければ登録されたものを生成
/// </summary>
[DisallowMultipleComponent]
[DefaultExecutionOrder(int.MinValue)]
public class SingletonGenerator : SingletonMonoBehaviour<SingletonGenerator>
{
    [SerializeField] List<GameObject> singletons = new List<GameObject>();

    private void Awake()
    {
        foreach(var i in singletons)
        {
            if (GameObject.Find(i.name)) continue;//同じ名前のオブジェクトがあれば保留
            Instantiate(i);
        }
    }
}
