using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopStartEnd : MonoBehaviour
{
    public Fungus.Flowchart flowchart;

    public GameObject ShopPrefab;
    /// <summary>
    /// ショップのプレハブを破壊する
    /// </summary>
    public void DestroyShopPrefab()
    {
        Destroy(ShopPrefab);
    }

    /// <summary>
    /// 終わるボタンが押されたときの処理
    /// </summary>
    public void PushEndButton()
    {
        flowchart.SendFungusMessage("EndShopping");
    }

}
