using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopMoneyUI : MonoBehaviour
{
    public Text text;
    private MoneyScript money;
    private void Start()
    {
        money = GameObject.Find("Managers").GetComponent<MoneyScript>();
        ChangeMoneyUI();
    }
    public void ChangeMoneyUI()
    {
        text.text=money.Money.ToString();
    }
}
