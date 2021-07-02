using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kyoichi;
public class ShopSellItemGenerator : MonoBehaviour
{
    //凍結中・・・invetnryのm_itemがpublicになるの待ち．



    //List<ItemSO> item = new List<ItemSO>();
    //public Inventry inventry;
    //private void Start()
    //{
    //    item = ShopLineup.GetItemLists();
    //    PrepareItem();

    //}
    ///// <summary>
    ///// 持ち物のアイテムを再度取得する．
    ///// アイテムの購入などにより所持しているアイテムが更新されたときに使う．
    ///// </summary>
    //public void reloadItemlist()
    //{

    //}
    ///// <summary>
    ///// ショップアイテムのリストにプレハブを生成．
    ///// </summary>
    //private void PrepareItem()
    //{
    //    GameObject prefab = (GameObject)Resources.Load("Shop/ShopItem");
    //    for (int i = 0; i < inventry.; i++)
    //    {
    //        GameObject instance = (GameObject)Instantiate(prefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
    //        instance.transform.parent = this.transform;
    //        instance.GetComponent<ShopItem>().itemdata = item[i];
    //    }
    //    Debug.Log("生成終わり？");

    //}
}
