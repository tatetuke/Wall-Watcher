using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuyItem : MonoBehaviour
{
    public ShopSelectItemManager selectManager;
    public Fungus.Flowchart flowchart;
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
