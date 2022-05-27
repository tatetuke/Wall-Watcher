using System.Collections;
using UnityEngine;

/// <summary>
/// ミニマップに表示されるクエストや採取ポイントのアイコン
/// </summary>
public class MinimapIcon : MonoBehaviour
{
    [Header("ゲームオブジェクト名から自動で取得")]
    [SerializeField, ReadOnly] private string iconName;

    public string IconName
    {
        get { return iconName; }
    }

    private void Awake()
    {
        iconName = gameObject.name;
    }
}