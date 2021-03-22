using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの位置とミニマップ上での位置を紐づける
/// </summary>
public class ScenePlaceInfo : MonoBehaviour
{
    [Header("ゲームオブジェクト名から自動で取得")]
    [SerializeField, ReadOnly] private string sceneName;

    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;

    /// <summary>0~1の値からミニマップ上での座標を返す</summary>
    /// <param name="progress">0~1の値　マップ上での進み具合</param>
    /// <returns>Minimap上での座標</returns>
    public Vector3 GetPlayerPosition(float progress)
    {
        return Vector3.zero;
    }
}
