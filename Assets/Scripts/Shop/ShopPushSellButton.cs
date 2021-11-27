using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kyoichi;
public class ShopPushSellButton : MonoBehaviour
{
    public ShopSelectItemManager selectManager;
    public Fungus.Flowchart flowchart;
    public Inventry inventry;
    private MoneyScript money;
    public ShopMoneyUI moneyUI;

    private void Start()
    {
        inventry = GameObject.Find("Managers").GetComponent<Inventry>();
        money = GameObject.Find("Managers").GetComponent<MoneyScript>();
    }
    /// <summary>
    /// 買うボタンが押されたときに発動する関数．
    /// </summary>
    public void PushSellButton()
    {
        if (selectManager.item == null)
        {
            CantSellItem();
        }else if (selectManager.sellItem.item.count==0)
        {
            NoSellItem();
        }
        else 
        {
            SellItem();
            selectManager.ChangeUIHasItemNum();
        }
    }
    /// <summary>
    /// アイテムを買う場合に発動する関数をまとめたもの．
    /// </summary>
    private void SellItem()
    {
        //アイテムの内部データ，表示を変更
        selectManager.sellItem.SellItem();
        //セーブデータの書き換え
        inventry.PopItem(selectManager.item);
        //店員が話す
        flowchart.SendFungusMessage("SellItem!");
        //お金を更新する
        UpdateMoney();
        //お金の表示の更新
        moneyUI.ChangeMoneyUI();
    }
    /// <summary>
    /// アイテムを買えない場合に発動する関数を纏めたもの．
    /// </summary>
    private void CantSellItem()
    {
        //店員が話す
        flowchart.SendFungusMessage("CantSellItem!");
    }

    /// <summary>
    ///アイテムが存在しない場合 
    /// </summary>
   private void NoSellItem()
    {
        //店員が話す.
        flowchart.SendFungusMessage("NoSellItem");
    }
    private void UpdateMoney()
    {
        money.Money = selectManager.item.price / 2 +money.Money;
    }
}
