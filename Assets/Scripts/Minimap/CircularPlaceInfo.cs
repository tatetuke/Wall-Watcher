using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各シーンにおけるプレイヤーの位置とミニマップ上での位置を紐づける
/// 円で構成されるマップ用
/// </summary>
public class CircularPlaceInfo: PlaceInfo
{
    [SerializeField] private float radius;
    [SerializeField,Tooltip("x軸正の向きからの角度")] private float startAngle;// x軸正の向きからの角度　0~360
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
        int dir = direction == Direction.CLOCKWISE ? -1 : 1;
        //内分点
        Vector3 pos = transform.position;
        pos.x += radius * Mathf.Cos(2 * Mathf.PI * progress * dir + startAngle / 180 * Mathf.PI);
        pos.y += radius * Mathf.Sin(2 * Mathf.PI * progress * dir + startAngle / 180 * Mathf.PI);
        return pos;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos.x += radius * Mathf.Cos(startAngle / 180 * Mathf.PI);
        pos.y += radius * Mathf.Sin(startAngle / 180 * Mathf.PI);

        Gizmos.color = Color.green;
        Utils.GizmosExtensions.DrawWireCircle(transform.position, radius,segments:30, rotation: Quaternion.LookRotation(Vector3.up));
        Gizmos.DrawLine(transform.position, pos);
        //円の接線を回る方向に出したい
        Vector3 tangentArrow = Vector3.zero;
        if (direction == Direction.CLOCKWISE)
        {
            tangentArrow.x = radius * Mathf.Sin(startAngle / 180 * Mathf.PI);
            tangentArrow.y = -radius * Mathf.Cos(startAngle / 180 * Mathf.PI);
        }
        else
        {
            tangentArrow.x = -radius * Mathf.Sin(startAngle / 180 * Mathf.PI);
            tangentArrow.y = radius * Mathf.Cos(startAngle / 180 * Mathf.PI);
        }
        Utils.GizmosExtensions.DrawArrow(pos, pos + tangentArrow*0.7f);
    }
#endif
}
