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

    public GameObject buyItemButton;
    public GameObject sellItemButton;
    public GameObject upgradeItemButton;

    public GameObject NoItemImage;

    public SelectTabEnum selectTab;  
    public enum SelectTabEnum
    {
        Buy,
        Sell,
        Upgrade
    }
    private void Start()
    {
        //初期状態では購入ボタンが押されている状態にする．
        ChangeSelectTabEnumBuy();
    }
    //タブが押されたときに呼ばれる関数(メイン)
    public void pushBuyTab()
    {
        UnactivateNoItemImage();
        ChangeSelectTabEnumBuy();
        ActivateBuyItemList();
        ActivateBuyItemButton();
    }
    public void pushSellTab()
    {
        UnactivateNoItemImage();
        ChangeSelectTabEnumSell();
        ActivateSellItemList();
        ActivateSellItemButton();
    }
    public void pushUpgradeTab()
    {
        UnactivateNoItemImage();
        ChangeSelectTabEnumUpgrade();
        ActivateUpgradeList();
        ActivateUpgradeItemButton();
    }

    //購入，(売却，強化)するアイテムのリストを生成する関数，
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
    //購入，(売却，強化)するアイテムのリストを生成する関数おわり

    //UI付近の購入(売却，強化)ボタンを出現させる関数．
    public void ActivateBuyItemButton()
    {
        UnactiveAllUIButton();
        buyItemButton.SetActive(true);
    }
    public void ActivateSellItemButton()
    {
        UnactiveAllUIButton();
        sellItemButton.SetActive(true);
    }
    public void ActivateUpgradeItemButton()
    {
        UnactiveAllUIButton();
        upgradeItemButton.SetActive(true);
    }
    //UI付近の購入(売却，強化)ボタンを出現させる関数．

    public void UnactveAllList()
    {
        sellItemList.SetActive(false);
        upgradeList.SetActive(false);
        buyItemList.SetActive(false);

    }
    public void UnactiveAllUIButton()
    {
        buyItemButton.SetActive(false);
        sellItemButton.SetActive(false);
        upgradeItemButton.SetActive(false);

    }
    public void ClearUI()
    {
        description.text = "";
        UIItemName.text = "";
        Icon.sprite = toumei;
       
    }

    //現在選択中のタブを変更するための関数3つ.
    /// <summary>
    /// 現在選択中のタブを"購入"に書き替える．
    /// </summary>
    public void ChangeSelectTabEnumBuy()
    {
        selectTab = SelectTabEnum.Buy;
    }
    /// <summary>
    /// 現在選択中のタブを"売却"に書き替える．
    /// </summary>
    public void ChangeSelectTabEnumSell()
    {
        selectTab = SelectTabEnum.Sell;
    }
    /// <summary>
    /// 現在選択中のタブを"強化"に書き替える．
    /// </summary>
    public void ChangeSelectTabEnumUpgrade()
    {
        selectTab = SelectTabEnum.Upgrade;
    }

    public void UnactivateNoItemImage()
    {
        NoItemImage.SetActive(false);
    }

}
