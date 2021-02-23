using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ItemContainerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemCountText;
    public UnityEvent OnClickItem { get; } = new UnityEvent();
    private void Awake()
    {
        var but = GetComponent<Button>();
        but.onClick.AddListener(OnClickItem.Invoke);
    }
    public void Initialize(Kyoichi.ItemStack item)
    {
        itemNameText.text = item.item.item_name;
        itemCountText.text = item.count.ToString();
    }
}
