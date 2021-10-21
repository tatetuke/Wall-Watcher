using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemGenerater : MonoBehaviour
{
    // Start is called before the first frame update
    public ShopLineUpSO ShopLineup;
    List<ItemSO> item = new List<ItemSO>();

    private void Start()
    {
        item = ShopLineup.GetItemLists();
        PrepareBuyItem();
       
    }

    /// <summary>
    /// ショップアイテムのリストにプレハブを生成．
    /// </summary>
    public void PrepareBuyItem()
    {
        GameObject prefab = (GameObject)Resources.Load("Shop/ShopItem");
        for (int i = 0; i < item.Count; i++)
        {
            GameObject instance=(GameObject)Instantiate(prefab,new Vector3(0f,0f,0f),Quaternion.identity);
            instance.transform.parent = this.transform;
            instance.GetComponent<ShopItem>().itemdata = item[i];
            //リストにあるアイテムの色を変える
            ChangeItemListColor(instance.GetComponent<Image>(), i);
        }
            Debug.Log("生成終わり");

    }
    private void ChangeItemListColor(Image image,int i)
    {
        if (i % 2 == 1)
        {
            image.color= new Color(0.85f, 0.8f, 0.8f, 1.0f);
        }

    }
    public void reloadBuyItemlist()
    {
        DestroyBuyItemList();
        PrepareBuyItem();
    }
    private void DestroyBuyItemList()
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
