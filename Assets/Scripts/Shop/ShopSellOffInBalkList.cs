using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ShopSellOffInBalkList : MonoBehaviour
{
  
    private static List<(ItemSO item,int num)>sellItemList  = new List<(ItemSO,int)>();
    private static List<(ItemSO item,int num)>sellOperateList  = new List<(ItemSO,int)>();

    static private TextMeshProUGUI SellListText;
    static public Text sumOfSellPriceText;

    static public int sumOfSellPrice;

    private void Start()
    {
        SellListText= GameObject.Find("ShopSellOffInBalkListUIText").GetComponent<TextMeshProUGUI>();
        sumOfSellPriceText = GameObject.Find("ShopSumOfSellPriceText").GetComponent<Text>();
    }
    //undoテスト用
    private void Update()
    {
        if (Input.GetKeyDown("z"))
        {
            if (ShopSellItemListToggle.toggle.isOn)
            {
                UndoSellItemlList();
            }
        }
    }


    static ShopSellOffInBalkList()
    {
    }

    /// <summary>
    /// 売却するアイテムをリストに入れる
    /// </summary>
    /// <param name="item">売却するアイテム</param>
    /// <param name="num">個数</param>
    public static void PushSellItemList(ItemSO item,int num)
    {
        //Undo用のリストを更新
        sellOperateList.Add((item, num));
        //売却合計金額を更新
        UpdateSellPriceNum(item,num);

        bool isContainedItemInList = false;
        //既にアイテムがリストに格納されていれば，リスト中の該当するアイテムの個数を更新する
        for(int i = 0; i < sellItemList.Count; i++)
        {
            if (sellItemList[i].item.name == item.name)
            {
                //(ItemSO , int) tmp=sellItemList[i]
                sellItemList[i] = (sellItemList[i].item, sellItemList[i].num + num);
                isContainedItemInList = true;
                break;
            }
        }
        //リストにアイテムが含まれなければ，新たにリストにアイテムを格納する．
        if(!isContainedItemInList)sellItemList.Add((item, num));


    }
    
    


    /// <summary>
    /// 売却リストの中身を削除(ex売却時
    /// </summary>
    public static void ClearSellItemList()
    {
        sellItemList.Clear();
    }

    /// <summary>
    /// 売却リストの表示を更新
    /// </summary>
    public static void SellItemListUIUpdate()
    {
        SellListText.text = "";
        int inLineSize = 0;
        for (int i = 0; i < sellItemList.Count; i++)
        {
            string nextPrintText = sellItemList[i].item.name +" x"+ sellItemList[i].num.ToString()+"  ";
            if(inLineSize+nextPrintText.Length>20)
            {
                SellListText.text += "\n";
                inLineSize = 0;
            }
            SellListText.text += nextPrintText;
            inLineSize += nextPrintText.Length;
        }
        UpdateSumOfSellPriceText();
    }
    public static void UpdateSumOfSellPriceText()
    {
        sumOfSellPriceText.text = sumOfSellPrice.ToString();
    }




    /// <summary>
    /// Undoしたときの処理
    /// </summary>
    public static void UndoSellItemlList()
    {
        if (sellOperateList.Count == 0) return;

        //Undoする操作
        (ItemSO item, int num) undoOperate =sellOperateList[sellOperateList.Count-1];


        UpdateSellPriceNum(undoOperate.item, -undoOperate.num);

        for (int i = 0; i < sellItemList.Count; i++)
        {
            if (sellItemList[i].item.name != undoOperate.item.name) continue;
            if (sellItemList[i].num - undoOperate.num == 0)
            {
                sellItemList.RemoveAt(i);
            }
            else
            {
                sellItemList[i] = (sellItemList[i].item, sellItemList[i].num - undoOperate.num);
            }
            SellItemListUIUpdate();
                break;
        }
        sellOperateList.RemoveAt(sellOperateList.Count - 1);

    }


    /// <summary>
    ///　売却時の取得金額の合計を更新
    /// </summary>
    /// <param name="item">対象のアイテムアイテム</param>
    /// <param name="num">個数</param>
    static private void UpdateSellPriceNum(ItemSO item,int num)
    {
        sumOfSellPrice += item.sellPrice * num;

    }
}
