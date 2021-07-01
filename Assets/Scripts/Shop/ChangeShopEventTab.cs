using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShopEventTab : MonoBehaviour
{
    public GameObject upgradeList;
    public GameObject buyItemList;
    public GameObject sellItemList;
    public void UnactveAllList()
    {
        sellItemList.SetActive(false);
        upgradeList.SetActive(false);
        buyItemList.SetActive(false);

    }
    public void ActivateUpgradeList()
    {
        UnactveAllList();
        upgradeList.SetActive(true);

    }
    public void ActivateBuyItemList()
    {
        UnactveAllList();
        buyItemList.SetActive(true);

    }
    public void ActivateSellItemList()
    {
        UnactveAllList();
        sellItemList.SetActive(true);
    }


}
