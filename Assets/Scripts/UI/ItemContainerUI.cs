using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Image))]
public class ItemContainerUI : MonoBehaviour
{

    [SerializeField] Sprite selectedImage;
    [SerializeField] Sprite unselectedImage;

    [Header("Refelences")]
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemCountText;
    [SerializeField] Image image;//アイテムの画像

    Animator animator;

    public UnityEvent OnClickItem { get; } = new UnityEvent();
    private void Awake()
    {
        animator = GetComponent<Animator>();
        var but = GetComponent<Button>();
        but.onClick.AddListener(OnClickItem.Invoke);
    }
    public void Initialize(Kyoichi.ItemStack item)
    {
        if(itemNameText)
        itemNameText.text = item.item.item_name;
        if(itemCountText)
        itemCountText.text = item.count.ToString();
    image.sprite=item.item.icon;
        var img = GetComponent<Image>();
        img.sprite = unselectedImage;
    }

    public void Select()
    {
        var img = GetComponent<Image>();
        img.sprite = selectedImage;
        animator.SetTrigger("select");
    }

    public void UnSelect()
    {
        var img = GetComponent<Image>();
        img.sprite = unselectedImage;
        animator.SetTrigger("unselect");
    }
}
