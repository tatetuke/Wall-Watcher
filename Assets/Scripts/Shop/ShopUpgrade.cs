using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kyoichi;
public class ShopUpgrade : MonoBehaviour
{
    [SerializeField] private GameObject updateDialog;
    private MoneyScript money;
    public ShopMoneyUI moneyUI;
    private ShopSelectItemManager selectManager;
    public Fungus.Flowchart flowchart;
    public Inventry inventry;
    public ChangeShopEventTab tabManager;
    private bool canUpgrade=true;

    [SerializeField] private ShopUpgradeItemGenerater generator;

    private void Start()
    {
        inventry = GameObject.Find("Managers").GetComponent<Inventry>();
        money = GameObject.Find("Managers").GetComponent<MoneyScript>();
        selectManager = GameObject.Find("ShopSelectItemManager").GetComponent<ShopSelectItemManager>();
    }

    //public void Select()
    //{
    //    if (!canUpgrade) return;
    //    if (selectManager.item == null)
    //    {
    //        NoItem();
    //    }
    //    else if (money.Money < selectManager.item.price)
    //    {
    //        CantUpgradeItem();
    //    }
    //    else
    //    {
    //        StartCoroutine(Upgrade());
    //        //Upgrade();
    //        //selectManager.ChangeUIHasItemNum();
    //    }
    //}

    public IEnumerator Select()
    {
        if (!canUpgrade) yield return 0;
        if (selectManager.item == null)
        {
            NoItem();
        }
        else if(money.Money< selectManager.item.price)
        {
            CantUpgradeItem();
        }
        else
        {
            yield return StartCoroutine(Upgrade());
            //Upgrade();
            //selectManager.ChangeUIHasItemNum();
        }
        yield return 0;
    }


    IEnumerator Upgrade()
    {

        //次のアップグレードを行えないようにする。
        canUpgrade = false;

        //セーブデータの書き換え
        inventry.PopItem(selectManager.item);
        inventry.AddItem(selectManager.item.afterUpdateItem);

        
        //yield return StartCoroutine(ShowUpgradeMessage());

        //店員が話す
        flowchart.SendFungusMessage("SellItem!");
        //お金を更新する
        UpdateMoney();
        //お金の表示の更新
        moneyUI.ChangeMoneyUI();

        //アイテムリストのリロード
        generator.reloadItemlist();

        //UIの初期化
        tabManager.ClearUI();
        //選択しているオブジェクトの初期化
        selectManager.ClearItemSelect();
        
        //次のアップグレードが行えるようにする。
        canUpgrade = true;

        yield return 0;

    }

    public void CantUpgradeItem()
    {
        //店員の会話
        flowchart.SendFungusMessage("NoMoney");

    }

    /// <summary>
    ///アイテムが存在しない場合 
    /// </summary>
    private void NoItem()
    {
        //店員が話す.
        flowchart.SendFungusMessage("CantSellItem!");
    }

    IEnumerator ShowUpgradeMessage()
    {
        updateDialog.SetActive(true);
        //連続入力を避ける
        yield return null;
        while (!Input.anyKeyDown) yield return null;
        updateDialog.SetActive(false);

        //連続入力を避ける
        int t = 0;
        while (t <= 3)
        {
            yield return null;
            t++;
        }
        yield return 0;
    }
    
    private void UpdateMoney()
    {
        money.Money -= selectManager.item.price;
    }

}
