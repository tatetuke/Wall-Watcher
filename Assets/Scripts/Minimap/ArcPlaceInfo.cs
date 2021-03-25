using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各シーンにおけるプレイヤーの位置とミニマップ上での位置を紐づける
/// 弧で構成されるマップ用
/// </summary>
public class ArcPlaceInfo : PlaceInfo
{
    [SerializeField] private CircleCollider2D circleWay;
    [Header("x軸から反時計回り方向")]
    [SerializeField,Range(-360,360)] private float startAngle;// x軸正の向きから反時計回り　0~360
    [SerializeField, Range(-360, 360)] private float endAngle;// x軸正の向きから反時計回り　0~360
    [SerializeField] private Direction direction;

    private enum Direction
    {
        CLOCKWISE,
        COUNTER_CLOCKWISE
    }

    /// <summary>0~1の値からミニマップ上での座標を返す</summary>
    /// <param name="progress">0~1の値　マップ上での進み具合</param>
    /// <returns>Minimap上での座標</returns>
    public override Vector3 GetPlayerPositionOnMinimap(float progress)
    {
        float c = Mathf.Clamp(progress, 0, 1);
        //内分点
        Vector3 pos = circleWay.transform.position;
        float startAng = (startAngle + 360) % 360;
        float endAng = (endAngle + 360) % 360;

        float rad = 0;
        if (direction == Direction.COUNTER_CLOCKWISE)
        {
            if (startAng < endAng)
                rad = startAng / 180 * Mathf.PI + (endAng - startAng) / 180 * Mathf.PI * c;
            else
                rad = startAng / 180 * Mathf.PI + (360 + endAng - startAng) / 180 * Mathf.PI * c;
        }
        else
        {
            if (startAng > endAng)
                rad = startAng / 180 * Mathf.PI + (endAng - startAng) / 180 * Mathf.PI * c;
            else
                rad = startAng / 180 * Mathf.PI + (-360 + endAng - startAng) / 180 * Mathf.PI * c;
        }

        pos.x += circleWay.radius * Mathf.Cos(rad);
        pos.y += circleWay.radius * Mathf.Sin(rad);
        return pos;
    }
}
