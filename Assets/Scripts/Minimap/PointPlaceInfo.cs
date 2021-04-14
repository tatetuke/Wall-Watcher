using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各シーンにおけるプレイヤーの位置とミニマップ上での位置を紐づける
/// 直線で構成されるマップ用
/// </summary>
public class PointPlaceInfo : PlaceInfo
{
    /// <summary>0~1の値からミニマップ上での座標を返す</summary>
    /// <param name="progress">0~1の値　マップ上での進み具合</param>
    /// <returns>Minimap上での座標</returns>
    public override Vector3 GetPlayerPositionOnMinimap(float progress)
    {
        return transform.position;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Utils.GizmosExtensions.DrawWireCircle(transform.position, 10, segments: 4, rotation: Quaternion.LookRotation(Vector3.up));
    }
#endif
}
