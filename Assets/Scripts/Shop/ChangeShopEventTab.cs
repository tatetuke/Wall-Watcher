using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShopEventTab : MonoBehaviour
{

    public GameObject upgradeList;
    public GameObject buyItemList;
    public GameObject sellItemList;
    public Fungus.Flowchart flowchart;

    public void UnactveAllList()
    {
        sellItemList.SetActive(false);
        upgradeList.SetActive(false);
        buyItemList.SetActive(false);

    }
    public void ActivateUpgradeList()
    {
        UnactveAllList();
        upgradeList.SetActive(true);
        //店員の会話を発動させる
        flowchart.SendFungusMessage("ShopUpgradeItem");
    }
    public void ActivateBuyItemList()
    {
        UnactveAllList();
        buyItemList.SetActive(true);
        //店員の会話を発動させる
        flowchart.SendFungusMessage("ShopBuyItem");

    }
    public void ActivateSellItemList()
    {
        UnactveAllList();
        sellItemList.SetActive(true);
        //店員の会話を発動させる
        flowchart.SendFungusMessage("ShopSellItem");
    }


}
