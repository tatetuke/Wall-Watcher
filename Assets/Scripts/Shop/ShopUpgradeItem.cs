using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Kyoichi;

public class ShopUpgradeItem : MonoBehaviour
{
    public ItemStack item;
    public ItemSO itemdata;

    GameObject descriptionObj;
    GameObject UIItemNameObj;
    GameObject IconObj;

    TextMeshProUGUI description;
    TextMeshProUGUI UIItemName;

    Image Icon;

    public Text ItemName;
    public Text priceText;
    //public GameObject blackImage;

    ShopSelectItemManager selectManager;


    // Start is called before the first frame update
    void Start()
    {
        //初期化
        descriptionObj = GameObject.Find("ShopUIItemDescription");
        IconObj = GameObject.Find("ShopUIItemIcon");
        UIItemNameObj = GameObject.Find("ShopUIItemName");
        selectManager = GameObject.Find("ShopSelectItemManager").GetComponent<ShopSelectItemManager>();

        Icon = IconObj.GetComponent<Image>();

        description = descriptionObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        UIItemName = UIItemNameObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        itemdata = item.item;
        ItemName.text = itemdata.item_name;

    }
    /// <summary>
    /// Itemのボタンが押されたときに呼ばれる関数
    /// </summary>
    public void PushItemButton()
    {

        ChangeSelectUpgradeItem();
        ChangeUI();
    }
    /// <summary>
    ///売却するアイテムを変更する．
    /// </summary>
    private void ChangeSelectUpgradeItem()
    {
        selectManager.item = itemdata;
    }
    /// <summary>
    ///UIのテキスト，アイコンを変更する関数．
    /// </summary>
    private void ChangeUI()
    {
        description.text = itemdata.description;
        description.text += "\n 必要素材：";
        for (int j=0 ; j < itemdata.requiredUpgradeItems.Count; j++)
        {
            ItemSO i = itemdata.requiredUpgradeItems[j];
            if (!selectManager.inventry.HasItem(i))
            {
                description.text += "<color=#ff0000>" + i.name + ":" + "1" + "</color>";
            }
            else description.text += i.name + ":" + "1";

            if(j!= itemdata.requiredUpgradeItems.Count-1)
            description.text += ", ";


        }
        //foreach (ItemSO i in itemdata.requiredUpgradeItems)
        //{
        //    if (!selectManager.inventry.HasItem(i))
        //    {
        //        description.text += "<color=#ff0000>" + i.name + ":" + "1" + "</color>";
        //    }
        //    else description.text += i.name+":"+"1";


        //    description.text += ", ";

        //}



        Icon.sprite = itemdata.icon;
        UIItemName.text = itemdata.item_name;
        selectManager.ChangeUIHasItemNum();

    }

   

}
