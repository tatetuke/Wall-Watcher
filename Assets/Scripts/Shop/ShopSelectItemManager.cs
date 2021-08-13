using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kyoichi;
public class ShopSelectItemManager : MonoBehaviour
{
    public ItemSO item;//選択中のアイテム
    public ShopSellItem sellItem;//売却するアイテム;
    /// <summary>
    /// 買うアイテムをリセット．
    /// ショップの購入，売却，強化モード切り替え時に呼び出される．
    /// </summary>
    public void ClearItemSelect()
    {
        item = null;
        sellItem=null;
    }
}
