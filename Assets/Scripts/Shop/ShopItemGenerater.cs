using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemGenerater : MonoBehaviour
{
    // Start is called before the first frame update
    public ShopLineUpSO ShopLineup;
    List<ItemSO> item = new List<ItemSO>();

    private void Start()
    {
        item = ShopLineup.GetItemLists();
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
        for (int i = 0; i < item.Count; i++)
        {
            GameObject instance=(GameObject)Instantiate(prefab,new Vector3(0f,0f,0f),Quaternion.identity);
            instance.transform.parent = this.transform;
            instance.GetComponent<ShopItem>().itemdata = item[i];
        }
            Debug.Log("生成終わり？");

    }
}
