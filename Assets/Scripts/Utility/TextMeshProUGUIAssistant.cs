using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// TextMeshProの補佐メソッドを提供する
/// </summary>
public class TextMeshProUGUIAssistant : MonoBehaviour
{
    [SerializeField, ReadOnly] private TextMeshProUGUI textMeshProUGUI;

    private void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }


    //Inspector上で TextMeshProUGUI.SetText(string)や拡張関数を呼び出せないための苦渋の策
    public void SetFloat(float value)
    {
        textMeshProUGUI.SetText(string.Format("{0:F2}", value));
    }
}

public static class TextMeshProUGUIExtension
{
    //Insupectorから呼び出せない　なんで・・・
    public static void SetFloat(this TextMeshProUGUI textMeshProUGUI, float value)
    {
        textMeshProUGUI.SetText(string.Format("{0:F2}", value));
    }
}

