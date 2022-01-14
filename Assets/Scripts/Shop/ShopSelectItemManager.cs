using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kyoichi;
using UnityEngine.UI;
public class ShopSelectItemManager : MonoBehaviour
{
    //static public ItemSO item;
    public ItemSO item;//選択中のアイテム
    public ShopSellItem sellItem;//売却するアイテム;
    public Inventry inventry;
    public Text numText;
    public GameObject numGameObject;
    private void Start()
    {
        inventry = GameObject.Find("Managers").GetComponent<Inventry>();
                
    }
    /// <summary>
    /// 買うアイテムをリセット．
    /// ショップの購入，売却，強化モード切り替え時に呼び出される．
    /// </summary>
    public void ClearItemSelect()
    {
        item = null;
        sellItem=null;
        ClearText();
    }
    public void ChangeUIHasItemNum()
    {
        numGameObject.SetActive(true);
        int num = inventry.DataCount(item);
        if (num == -1) num = 0;
        numText.text =num.ToString();
        
    }
    public void ClearText()
    {
        numGameObject.SetActive(false);
    }
}
