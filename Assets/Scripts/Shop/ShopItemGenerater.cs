using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemGenerater : MonoBehaviour
{
    // Start is called before the first frame update
    public ShopLineUpSO ShopLineup;
    List<ItemSO> Item = new List<ItemSO>();

    private void Start()
    {
        Item = ShopLineup.GetItemLists();
        PrepareItem();
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ショップアイテムのリストにプレハブを生成．
    /// </summary>
    private void PrepareItem()
    {
        GameObject prefab = (GameObject)Resources.Load("Shop/ShopItem");
        for (int i = 0; i < Item.Count; i++)
        {
            GameObject instance=(GameObject)Instantiate(prefab,new Vector3(0f,0f,0f),Quaternion.identity);
            instance.transform.parent = this.transform;
            instance.GetComponent<ShopItem>().itemdata = Item[i];
            //instance.GetComponent<ShopItem>().ItemName.text = Item[i].item_name;
        }
            Debug.Log("生成終わり？");

    }
}
