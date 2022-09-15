﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Map上での位置や名前を知るためのクラス
/// </summary>
class LocationGetter : SingletonMonoBehaviour<LocationGetter>
{
    [SerializeField] private string currentPlaceName;
    [SerializeField] private int currentFloor;
    [SerializeField] private GameObject playerObject;
    [Header("マップ端の座標")]
    [SerializeField] private Transform startPosition; //Minimap上のstartPositionとendPositionを揃えないと向きが逆になるので注意
    [SerializeField] private Transform endPosition;
    [Header("currentPlaceNameをシーン名から自動取得するか")]
    [SerializeField] private bool autoPlaceName;

    public string CurrentPlaceName
    {
        get { return currentPlaceName; }
        set { currentPlaceName = value; }
    }
    public int CurrentFloor
    {
        get { return currentFloor; }
        set { currentFloor = value; }
    }

    private void Awake()
    {
        if (autoPlaceName)
        {
            currentPlaceName = SceneManager.GetActiveScene().name;
        }
    }

    public float GetProgressOnMap()
    {
        Vector3 position = playerObject.transform.position;
        //内分点を探す
        return Mathf.Clamp((position.x - startPosition.position.x) / (endPosition.position.x - startPosition.position.x), 0, 1);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Utils.GizmosExtensions.DrawArrow(startPosition.position, endPosition.position);
    }
#endif
}

