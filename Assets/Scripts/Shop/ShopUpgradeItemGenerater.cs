using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kyoichi;
public class ShopUpgradeItemGenerater : MonoBehaviour
{
    public Inventry inventry;
    public GameObject NoSellItemImage;
    private void Start()
    {
        //inventry = GameObject.Find("Managers").GetComponent<Inventry>();
        //PrepareItem();

    }
    /// <summary>
    /// 持ち物のアイテムを再度取得する．
    /// アイテムの購入などにより所持しているアイテムが更新されたときに使う．
    /// </summary>
    public void reloadItemlist()
    {
        inventry = GameObject.Find("Managers").GetComponent<Inventry>();
        DestroyItemList();
        PrepareItem();
    }
    /// <summary>
    /// ショップアイテムのリストにプレハブを生成．
    /// </summary>
    private void PrepareItem()
    {
        GameObject prefab = (GameObject)Resources.Load("Shop/ShopUpgradeItem");

        bool IsSetItem = false;//アイテムが生成されたかどうかを管理する変数．

        int i = 0;
        foreach (var item in inventry.Data)
        {
            if (item.item == null) Debug.LogError("DataにあるSOのAftarUpdateItemがセットされていない可能性がある");
            if (item.item.canUpgradeItem == false) continue;
            if (item.count == 0) continue;
            if (item.item.afterUpdateItem == null) continue;
            IsSetItem = true;
            ItemStack tmpItem = new ItemStack(item.item, item.count);
            GameObject instance = (GameObject)Instantiate(prefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            instance.transform.parent = this.transform;
            instance.GetComponent<ShopUpgradeItem>().item = tmpItem;
            instance.transform.GetChild(0).GetComponent<Text>().text = item.item.name;
            ChangeItemListColor(instance.GetComponent<Image>(), i);
            i++;
        }
        Debug.Log("アイテムリスト生成完了");

        //アイテムが生成されなかったら
        if (IsSetItem == false) ActivateNoSellItemImage();

    }
    private void ChangeItemListColor(Image image, int i)
    {
        if (i % 2 == 1)
        {
            image.color = new Color(0.85f, 0.8f, 0.8f, 1.0f);
        }

    }
    public void DestroyItemList()
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ActivateNoSellItemImage()
    {
        NoSellItemImage.SetActive(true);
    }

}
