using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// セーブ・ロードをテストするLoadTestシーンのマネージャー
/// </summary>
public class GameManager_Kyoichi : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        //データをロードするときはSaveLoadManager.LoadをStartもしくはUpdate内で行ってください。
        //Awakeでは行わないよう
        SaveLoadManager.Instance.Load();
    }
    private void OnApplicationQuit()
    {
        //ゲームを終了したときに自動でセーブされるようになってます
        SaveLoadManager.Instance.Save();
    }
}
