using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventryUI : MonoBehaviour
{
    [SerializeField] GameObject itemContainerUI;
    [SerializeField] RectTransform container;
    [SerializeField] TMP_Dropdown dropdown;
    private void Start()
    {
        dropdown.onValueChanged.AddListener(OnSelectRarelity);
    }
    public void OnSelectRarelity(int value)
    {

    }

    public void Initialize()
    {

    }

}
