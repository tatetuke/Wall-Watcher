using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuyItem : MonoBehaviour
{
    public ItemSO item;//買うアイテム．
    public Fungus.Flowchart flowchart;
    /// <summary>
    /// 買うボタンが押されたときに発動する関数．
    /// </summary>
    public void PushBuyButton()
    {
        if (item == null)
        {
            CantBuyItem();
        }
        else
        {
            BuyItem();
        }
    }
    /// <summary>
    /// アイテムを買う場合に発動する関数．
    /// </summary>
    private void BuyItem()
    {
        //店員が話す
        flowchart.SendFungusMessage("BuyItem!");

    }
    private void CantBuyItem()
    {
        //店員が話す
        flowchart.SendFungusMessage("CantBuyItem!");
    }
    /// <summary>
    /// 買うアイテムをリセットします．
    /// ショップの購入，売却，強化モード切り替え時に呼び出される．
    /// </summary>
    public void ClearItemSelect()
    {
        item = null;
    }
}
