using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Map上での位置や名前を知るためのクラス
/// </summary>
class LocationGetter : SingletonMonoBehaviour<LocationGetter>
{
    [SerializeField] private string currentPlaceName;
    [Header("マップ端の座標")]
    [SerializeField] private Transform startPosition; //Minimap上のstartPositionとendPositionを揃えないと向きが逆になるので注意
    [SerializeField] private Transform endPosition;

    public string CurrentPlaceName
    {
        get { return currentPlaceName; }
    }

    public float GetProgressOnMap(Vector3 position)
    {
        //内分点を探す
        return Mathf.Clamp((position.x - startPosition.position.x) / (endPosition.position.x - startPosition.position.x), 0, 1);
    }


}

