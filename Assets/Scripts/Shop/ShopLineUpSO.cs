using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopLineup", menuName = "ScriptableObject/ShopLineup")]
public class ShopLineUpSO : ScriptableObject
{
	string chapter;//このShopLineupが使用される物語のチャプター
	[SerializeField]
	private List<ItemSO> itemLists = new List<ItemSO>();
	public List<ItemSO> GetItemLists()
	{
		return itemLists;
	}
}
