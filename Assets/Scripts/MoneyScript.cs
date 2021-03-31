using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所持金を管理するクラス
/// </summary>
[DisallowMultipleComponent]
public class MoneyScript : MonoBehaviour
{
    [SerializeField] int defaultMoney;//初期値
    [SerializeField] int maxMoney;//お金の最大値
    [SerializeField,ReadOnly] int currentMoney;//現在の所持金
    public int Money//現在の所持金
    {
        get
        {
            return currentMoney;
        }
        set
        {
            currentMoney = Mathf.Clamp(value, 0, maxMoney);
        }
    }
    private void Awake()
    {
        //クエストなどで所持金を参照するためにGamePropertyManagerに登録
        GamePropertyManager.Instance?.RegisterParam("money", () => Money);
        Kyoichi.GameManager.Instance.OnGameLoad.AddListener(Load);
        Kyoichi.GameManager.Instance.OnGameSave.AddListener(Save);
    }
    public void Load()
    {
        //値をセーブファイルからロード
       // Money = PropertyLoader.Instance.GetInt("money", defaultMoney);
    }
    public void Save()
    {
        //値をファイルに保存するために、ゲームが終わる直前の値を保存
      //  PropertyLoader.Instance.SetInt("money", Money);
    }

}
