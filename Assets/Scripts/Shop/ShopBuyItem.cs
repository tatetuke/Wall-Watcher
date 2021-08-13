using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kyoichi;
public class ShopBuyItem : MonoBehaviour
{
    public ShopSelectItemManager selectManager;
    public Fungus.Flowchart flowchart;
    public Inventry inventry;

    private void Start()
    {
        inventry=GameObject.Find("Managers").GetComponent<Inventry>();
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
        else
        {
            BuyItem();
            //ここでセーブデータに書き込み
            inventry.AddItem(selectManager.item);
        }
    }
    /// <summary>
    /// アイテムを買う場合に発動する関数を纏めたもの．
    /// </summary>
    private void BuyItem()
    {
        //店員が話す
        flowchart.SendFungusMessage("BuyItem!");

    }
    /// <summary>
    /// アイテムを買えない場合に発動する関数を纏めたもの．
    /// </summary>
    private void CantBuyItem()
    {
        //店員が話す
        flowchart.SendFungusMessage("CantBuyItem!");
    }

}
