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
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI description;
    [Header("debug")]
    [SerializeField, ReadOnly] Kyoichi.Inventry target;

    ItemContainerUI selectedContainer;

    private void Awake()
    {
        OnViewShow.AddListener(Initialize);
        OnViewHided.AddListener(() =>
        {
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
            itemIcon.sprite = null;
            itemName.text = "";
            description.text = "";
        });
    }

    public void OnSelectRarelity(int value){}

    public void Initialize()
    {
        target = FindObjectOfType<Kyoichi.Inventry>();
        if (target == null)
        {
            Debug.Log("target not found");
            return;
        }
        foreach (var i in target.Data)
        {
            var obj = Instantiate(itemContainerUI, container).GetComponent<ItemContainerUI>();
            obj.Initialize(i);
            obj.OnClickItem.AddListener(() =>
            {
                if (selectedContainer) selectedContainer.UnSelect();
                SelectItem(i.item);
                obj.Select();
                selectedContainer = obj;
            });
        }
    }

    void SelectItem(ItemSO target)
    {
        Debug.Log(target.icon);
        itemIcon.sprite = target.icon;
        itemName.text = target.item_name;
        description.text = target.description;
    }
}
