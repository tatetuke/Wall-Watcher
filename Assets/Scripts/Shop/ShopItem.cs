using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopItem : MonoBehaviour
{
    public ItemSO itemdata;
    GameObject descriptionObj;
    GameObject UIItemNameObj;
    GameObject IconObj;
    TextMeshProUGUI description;
    TextMeshProUGUI UIItemName;
    Image Icon;
    Text ItemName;
    ShopSelectItemManager selectManager;

    // Start is called before the first frame update
    void Start()
    {
    
        descriptionObj = GameObject.Find("ShopUIItemDescription");
        IconObj = GameObject.Find("ShopUIItemIcon");
        UIItemNameObj = GameObject.Find("ShopUIItemName");
        selectManager= GameObject.Find("ShopSelectItemManager").GetComponent<ShopSelectItemManager>();

        ItemName = this.gameObject.transform.GetChild(0).GetComponent<Text>();
        Icon = IconObj.GetComponent<Image>();
        description = descriptionObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        UIItemName = UIItemNameObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        ItemName.text = itemdata.item_name;
    }
    /// <summary>
    /// Itemのボタンが押されたときに呼ばれる関数
    /// </summary>
    public void PushItemButton()
    {
        ChangeUI();
        ChangeSelectBuyItem();
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
        UIItemName.text = itemdata.item_name;
    }
}
