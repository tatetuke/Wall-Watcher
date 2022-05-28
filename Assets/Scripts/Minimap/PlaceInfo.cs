using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各シーンにおけるプレイヤーの位置とミニマップ上での位置を紐づける
/// </summary>
public  abstract class PlaceInfo : MonoBehaviour
{
    [Header("ゲームオブジェクト名から自動で取得")]
    [SerializeField, ReadOnly] private string placeName;
    public string PlaceName
    {
        get { return placeName; }
    }


    protected virtual void Awake()
    {
        placeName = gameObject.name;
    }

    /// <summary>0~1の値からミニマップ上での座標を返す</summary>
    /// <param name="progress">0~1の値　マップ上での進み具合</param>
    /// <returns>Minimap上での座標</returns>
    abstract public Vector3 GetPlayerPositionOnMinimap(float progress);
}
