using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kyoichi;
public class ShopSellItemGenerator : MonoBehaviour
{
    public Inventry inventry;
    private void Start()
    {
        inventry = GameObject.Find("Managers").GetComponent<Inventry>();
        //PrepareItem();
    }
    /// <summary>
    /// 持ち物のアイテムを再度取得する．
    /// アイテムの購入などにより所持しているアイテムが更新されたときに使う．
    /// </summary>
    public void reloadItemlist()
    {
        DestroyItemList();
        PrepareItem();
    }
    /// <summary>
    /// ショップアイテムのリストにプレハブを生成．
    /// </summary>
    private void PrepareItem()
    {
        GameObject prefab = (GameObject)Resources.Load("Shop/ShopSellItem");

        int i = 0;
        foreach (var item in inventry.Data)
        {
            if (item.item.canSellItem == false) continue;
            if (item.count == 0) continue;
            else if (item.count < 0)
            {
                Debug.LogError("アイテムの数が負の数です");
                continue;
            }

            ItemStack tmpItem = new ItemStack(item.item, item.count);
            GameObject instance = (GameObject)Instantiate(prefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            instance.transform.parent = this.transform;
            instance.GetComponent<ShopSellItem>().item = tmpItem;
            ChangeItemListColor(instance.GetComponent<Image>(), i);
            i++;
        Debug.Log("売却アイテム生成");
        }
        Debug.Log("売却アイテム生成おわり");

    }
    private void ChangeItemListColor(Image image, int i)
    {
        if (i % 2 == 1)
        {
            image.color = new Color(0.85f, 0.8f, 0.8f, 1.0f);
        }

    }
    //タブを切り替えた際に，アイテムリストを削除する関数．
    public void DestroyItemList()
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
