using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kyoichi;
public class ShopBuyItem : MonoBehaviour
{
    public ShopSelectItemManager selectManager;
    public Fungus.Flowchart flowchart;
    public Inventry inventry;
    private MoneyScript money;
    private void Start()
    {
        inventry=GameObject.Find("Managers").GetComponent<Inventry>();
        money=GameObject.Find("Managers").GetComponent<MoneyScript>();
    }

    /// <summary>
    /// 買うボタンが押されたときに発動する関数．
    /// </summary>
    public void PushBuyButton()
    {
        
        if (selectManager.item == null)
        {
            CantBuyItem();
        }
        else if (money.Money-selectManager.item.price<0)
        {
            NoMoney();
        }
        else
        {
            BuyItem();
            selectManager.ChangeUIHasItemNum();

        }
    }
    /// <summary>
    /// アイテムを買う場合に発動する関数を纏めたもの．
    /// </summary>
    private void BuyItem()
    {
        //店員が話す
        flowchart.SendFungusMessage("BuyItem!");
        //ここでセーブデータに書き込み
        inventry.AddItem(selectManager.item);
        //所持金の更新
        money.Money -= selectManager.item.price;

    }
    /// <summary>
    /// アイテムを買えない場合に発動する関数を纏めたもの．
    /// </summary>
    private void CantBuyItem()
    {
        //店員が話す
        flowchart.SendFungusMessage("CantBuyItem!");
    }
    /// <summary>
    /// 買うお金がない場合に発動する関数を纏めたもの．
    /// </summary>
    private void NoMoney()
    {
        //店員が話す
        flowchart.SendFungusMessage("CantBuyNoMoney");
    }

}
