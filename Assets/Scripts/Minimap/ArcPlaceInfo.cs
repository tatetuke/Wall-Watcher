using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各シーンにおけるプレイヤーの位置とミニマップ上での位置を紐づける
/// 弧で構成されるマップ用
/// </summary>
public class ArcPlaceInfo : PlaceInfo
{
    [SerializeField] private float radius;
    [SerializeField, Range(-360, 360)] private float startAngle;// x軸正の向きから反時計回り　-360~360
    [SerializeField] private float arcMeasure; //反時計回り

    /// <summary>0~1の値からミニマップ上での座標を返す</summary>
    /// <param name="progress">0~1の値　マップ上での進み具合</param>
    /// <returns>Minimap上での座標</returns>
    public override Vector3 GetPlayerPositionOnMinimap(float progress)
    {
        float c = Mathf.Clamp(progress, 0, 1);
        //内分点
        Vector3 pos = transform.position;
        float startAng = (startAngle + 360) % 360;
        float endAng = (startAngle + arcMeasure + 360) % 360;

        float rad = 0;
        if (arcMeasure >= 0)
        {
            //反時計回り
            if (startAng < endAng)
                rad = startAng / 180 * Mathf.PI + (endAng - startAng) / 180 * Mathf.PI * c;
            else
                rad = startAng / 180 * Mathf.PI + (360 + endAng - startAng) / 180 * Mathf.PI * c;
        }
        else
        {
            //時計回り
            if (startAng > endAng)
                rad = startAng / 180 * Mathf.PI + (endAng - startAng) / 180 * Mathf.PI * c;
            else
                rad = startAng / 180 * Mathf.PI + (-360 + endAng - startAng) / 180 * Mathf.PI * c;
        }

        pos.x += radius * Mathf.Cos(rad);
        pos.y += radius * Mathf.Sin(rad);
        return pos;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 startPos = transform.position;
        startPos.x += radius * Mathf.Cos(startAngle / 180 * Mathf.PI);
        startPos.y += radius * Mathf.Sin(startAngle / 180 * Mathf.PI);
        Vector3 endPos = transform.position;
        endPos.x += radius * Mathf.Cos((startAngle + arcMeasure) / 180 * Mathf.PI);
        endPos.y += radius * Mathf.Sin((startAngle + arcMeasure) / 180 * Mathf.PI);

        Gizmos.color = Color.green;
        Quaternion rotation = Quaternion.Euler(new Vector3(-startAngle, 90, 90));

        int segments = 20;
        Utils.GizmosExtensions.DrawWireArc(transform.position, radius, arcMeasure, Mathf.Abs(Mathf.FloorToInt(360 / arcMeasure * segments)), rotation);
        Gizmos.DrawLine(transform.position, startPos);
        Gizmos.DrawLine(transform.position, endPos);
        //円の接線を回る方向に出したい
        Vector3 tangentArrow = Vector3.zero;
        if (arcMeasure >= 0)
        {
            //反時計回り
            tangentArrow.x = -radius * Mathf.Sin(startAngle / 180 * Mathf.PI);
            tangentArrow.y = radius * Mathf.Cos(startAngle / 180 * Mathf.PI);
        }
        else
        {
            //時計回り
            tangentArrow.x = radius * Mathf.Sin(startAngle / 180 * Mathf.PI);
            tangentArrow.y = -radius * Mathf.Cos(startAngle / 180 * Mathf.PI);
        }
        Utils.GizmosExtensions.DrawArrow(startPos, startPos + tangentArrow*0.7f);
    }
#endif
}
