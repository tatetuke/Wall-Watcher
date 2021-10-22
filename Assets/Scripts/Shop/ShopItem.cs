using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Kyoichi;
public class ShopItem : MonoBehaviour
{
    public ItemSO itemdata;
    
    GameObject descriptionObj;
    GameObject uiItemNameObj;
    GameObject iconObj;
    
    TextMeshProUGUI description;
    TextMeshProUGUI uiItemName;

    Image Icon;

    Text itemName;
    Text priceText;
    
    ShopSelectItemManager selectManager;

    MoneyScript inventryMoney;


    // Start is called before the first frame update
    void Start()
    {
        Init();
        ChangePriceColorAsInventryMoney();

    }
    /// <summary>
    /// Itemのボタンが押されたときに呼ばれる関数
    /// </summary>
    public void PushItemButton()
    {
        ChangeSelectBuyItem();
        ChangeUI();
        ChangePriceColorAsInventryMoney();
    }
    /// <summary>
    ///購入するアイテムを変更する．
    /// </summary>
    private void ChangeSelectBuyItem()
    {
        selectManager.item = itemdata;
    }
    /// <summary>
    ///UIのテキスト，アイコンを変更する関数．
    /// </summary>
    private void ChangeUI()
    {
        description.text = itemdata.description;
        Icon.sprite = itemdata.icon;
        uiItemName.text = itemdata.item_name;
        selectManager.ChangeUIHasItemNum();

    }
    
    /// <summary>
    /// お金が足りなくなったときに色を変化させる
    /// </summary>
    private void ChangePriceColorAsInventryMoney()
    {
        if (inventryMoney.Money < itemdata.price)
        {
            priceText.color = Color.red;
        }
        else
        {
            priceText.color = Color.black;
        }

    }

    //初期化
    private void Init()
    {
        descriptionObj = GameObject.Find("ShopUIItemDescription");
        iconObj = GameObject.Find("ShopUIItemIcon");
        uiItemNameObj = GameObject.Find("ShopUIItemName");
        selectManager = GameObject.Find("ShopSelectItemManager").GetComponent<ShopSelectItemManager>();
        inventryMoney = GameObject.Find("Managers").GetComponent<MoneyScript>();


        itemName = this.gameObject.transform.GetChild(0).GetComponent<Text>();
        priceText = this.gameObject.transform.GetChild(1).GetComponent<Text>();
        Icon = iconObj.GetComponent<Image>();
        description = descriptionObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        uiItemName = uiItemNameObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        itemName.text = itemdata.item_name;
        priceText.text = itemdata.price.ToString();
    }
}
