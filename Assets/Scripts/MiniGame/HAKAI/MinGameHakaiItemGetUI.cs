using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MinGameHakaiItemGetUI : MonoBehaviour
{
    [SerializeField] private List<Image> ItemBox;
    [SerializeField] private MingameHAKAIGetItemManager ItemManager;
    [SerializeField] private Sprite toumeiImage;
    /// <summary>
    /// UIのゲットしたアイテムの欄を更新する関数
    /// </summary>
    public void ChangeGetItemUI()
    {
        
        int t = 0;
        for(int i = 0; i < ItemManager.Item.Count; i++)
        {
            //Itemがゲットできない状態だったらcontinue;
            if (!ItemManager.Item[i].GetComponent<MinGameHAKAIItem>().CanGetItem) continue;
            //ItemUIを更新
            ItemBox[t].sprite = ItemManager.Item[i].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
            t++;
        }
        while (t<ItemBox.Count)
        {
            ItemBox[t].sprite = toumeiImage;
            t++;
        }
    }

}
