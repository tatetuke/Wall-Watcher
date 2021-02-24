using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class InventryUI : UIView
{
    [SerializeField] GameObject itemContainerUI;
    [Header("Object references")]
    [SerializeField] RectTransform container;
    //[SerializeField] TMP_Dropdown dropdown;
    [SerializeField] Dropdown dropdown;
    [SerializeField] Kyoichi.Inventry target;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI description;
    private void Start()
    {
        dropdown.onValueChanged.AddListener(OnSelectRarelity);
        OnViewShow.AddListener(Initialize);
        OnViewHide.AddListener(()=> {
            foreach(Transform child in container)
            {
                Destroy(child.gameObject);
            }
            itemIcon.sprite = null;
            itemName.text = "";
            description.text ="";
        });
    }
    public void OnSelectRarelity(int value)
    {

    }
    public void Initialize()
    {
        foreach(var i in target.Data)
        {
            var obj = Instantiate(itemContainerUI, container).GetComponent<ItemContainerUI>();
            obj.Initialize(i);
            obj.OnClickItem.AddListener(() =>
            {
                SelectItem(i.item);
            });
        }
    }

    void SelectItem(ItemSO target)
    {
        itemIcon.sprite = target.icon;
        itemName.text = target.item_name;
        description.text = target.description;
    }
}
