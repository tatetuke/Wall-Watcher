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

    // Start is called before the first frame update
    void Start()
    {
        ItemName = this.gameObject.transform.GetChild(0).GetComponent<Text>();
        descriptionObj = GameObject.Find("ShopUIItemDescription");
        IconObj = GameObject.Find("ShopUIItemIcon");
        UIItemNameObj = GameObject.Find("ShopUIItemName");
        Icon = IconObj.GetComponent<Image>();
        description = descriptionObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Debug.Log("アイテム生成");
        UIItemName = UIItemNameObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (itemdata == null) Debug.Log("error");
        if (description == null) Debug.Log("error1");
        ItemName.text = itemdata.item_name;
    }

    /// <summary>
    ///UIのテキスト，アイコンを変更する関数．
    /// </summary>
    public void ChangeUI()
    {
        description.text = itemdata.description;
        Icon.sprite = itemdata.icon;
        UIItemName.text = itemdata.item_name;
    }
}
