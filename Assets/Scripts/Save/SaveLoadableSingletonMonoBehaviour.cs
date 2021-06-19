using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

/// <summary>
/// セーブ・ロードを行うための抽象クラス
/// SaveLoadManagerから一括で呼ばれる
/// </summary>
public abstract class SaveLoadableSingletonMonoBehaviour<T> : SaveLoadableMonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    Debug.LogError(typeof(T) + " is nothing");
                }
            }

            return instance;
        }
    }
}