using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各シーンにおけるプレイヤーの位置とミニマップ上での位置を紐づける
/// 直線で構成されるマップ用
/// </summary>
public class LinearPlaceInfo : PlaceInfo
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;

    /// <summary>0~1の値からミニマップ上での座標を返す</summary>
    /// <param name="progress">0~1の値　マップ上での進み具合</param>
    /// <returns>Minimap上での座標</returns>
    public override Vector3 GetPlayerPositionOnMinimap(float progress)
    {
        float c = Mathf.Clamp(progress, 0, 1);
        //内分点
        Vector3 pos = startPosition.position * (1-c) + endPosition.position * c;
        return pos;
    }
}
