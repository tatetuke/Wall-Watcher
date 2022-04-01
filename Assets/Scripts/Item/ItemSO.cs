using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObject/Item")]
public class ItemSO : ScriptableObject
{
    public enum Rarelity
    {
        N,//地面に植えられる
        R,
        SR,
        SSR,
    }
    [Tooltip("実際にゲーム内で表示される名前")]
    public string item_name;
    [Multiline]
    public string description;
    public Sprite icon;
    public Rarelity type;
    public int price;
    public int sellPrice;
    public bool canUpgradeItem;
    public bool canSellItem;
    public List<ItemSO>requiredUpgradeItems= new List<ItemSO>();
    [Tooltip("アップデート後のアイテム(アップデートできないアイテムはnull")]
    public ItemSO afterUpdateItem;

}
