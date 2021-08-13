using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPushUpgradeButton : MonoBehaviour
{
    public ShopSelectItemManager selectManager;
    public Fungus.Flowchart flowchart;
    /// <summary>
    /// 買うボタンが押されたときに発動する関数．
    /// </summary>
    public void PushUpgradeButton()
    {
        if (selectManager.item == null)
        {
            CantUpgradeItem();
        }
        else
        {
            UpgradeItem();
        }
    }
    /// <summary>
    /// アイテムを買う場合に発動する関数を纏めたもの．
    /// </summary>
    private void UpgradeItem()
    {
        //店員が話す
        flowchart.SendFungusMessage("UpgradeItem!");

    }
    /// <summary>
    /// アイテムを買えない場合に発動する関数を纏めたもの．
    /// </summary>
    private void CantUpgradeItem()
    {
        //店員が話す
        flowchart.SendFungusMessage("CantUpgradeItem!");
    }
}
