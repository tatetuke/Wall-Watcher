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
public abstract class SaveLoadableSingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
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

    protected abstract void Save();
    protected abstract void Load();
    protected abstract UniTask SaveAsync();
    protected abstract UniTask LoadAsync();

    /// <summary>
    /// 内部で使用する予定のkeyのList
    /// </summary>
    protected abstract List<string> GetKeyList();

    //overrideするときは base.Awake()を呼ぶこと
    //内容はkeyの確認とSaveLoadManagerへの登録
    protected virtual void Awake()
    {
        var usedKeyList = GetKeyList();
        if (usedKeyList.Count == 0)
        {
            Debug.LogWarning("usedKeyList.Count is 0. Add Key used in Save&Load");
        }

        //使用済みのkeyがあれば知らせる
        if (!SaveLoadManager.Instance.CheckKeyListAvailable(usedKeyList))
        {
            //シーン遷移後などに同じkeyが登録される場合がよくある どうしよう・・
            //foreach(string key in usedKeyList)
            //{
            //    if (!SaveLoadManager.Instance.CheckKeyAvailable(key))
            //    {
            //        Debug.LogError($"The key '{key}' is already exist. Try setting different key");
            //    }
            //}
            return;
        }

        //keyの登録
        SaveLoadManager.Instance.AddKeyList(usedKeyList);
        SaveLoadManager.Instance.AddSaveCallBack(Save);
        SaveLoadManager.Instance.AddLoadCallBack(Load);
        SaveLoadManager.Instance.AddLoadCallBack(SaveAsync());
        SaveLoadManager.Instance.AddLoadCallBack(LoadAsync());
    }
}