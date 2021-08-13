using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChangeShopEventTab : MonoBehaviour
{
    /// <summary>
    /// 選択中のUIのアイコン，テキスト
    /// </summary>
    public TextMeshProUGUI description;
    public TextMeshProUGUI UIItemName;
    public Image Icon;
    public Sprite toumei;

    public GameObject upgradeList;
    public GameObject buyItemList;
    public GameObject sellItemList;
    public Fungus.Flowchart flowchart;

 
    public void ActivateUpgradeList()
    {
        ClearUI();
        UnactveAllList();
        upgradeList.SetActive(true);
        //店員の会話を発動させる
        flowchart.SendFungusMessage("ShopUpgradeItem");
    }
    public void ActivateBuyItemList()
    {
        ClearUI();
        UnactveAllList();
        buyItemList.SetActive(true);
        //店員の会話を発動させる
        flowchart.SendFungusMessage("ShopBuyItem");

    }
    public void ActivateSellItemList()
    {
        ClearUI();
        UnactveAllList();
        sellItemList.SetActive(true);
        //店員の会話を発動させる
        flowchart.SendFungusMessage("ShopSellItem");
    }
    public void UnactveAllList()
    {
        sellItemList.SetActive(false);
        upgradeList.SetActive(false);
        buyItemList.SetActive(false);

    }
    public void ClearUI()
    {
        description.text = "";
        UIItemName.text = "";
        Icon.sprite = toumei;
    }

}
