using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopStartEnd : MonoBehaviour
{
    public Fungus.Flowchart flowchart;
    private bool EndShop=false;
    public GameObject ShopPrefab;
    private void Update()
    {
        if (Input.GetKeyDown("escape")&&!EndShop)
        {
            PushEndButton();
        }
    }
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
        EndShop = true;
        flowchart.SendFungusMessage("EndShopping");
    }

}
