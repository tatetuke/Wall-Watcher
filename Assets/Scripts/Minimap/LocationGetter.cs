using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Map上での位置や名前を知るためのクラス
/// </summary>
class LocationGetter : SingletonMonoBehaviour<LocationGetter>
{
    [SerializeField] private string currentPlaceName;
    [SerializeField] private GameObject playerObject;
    [Header("マップ端の座標")]
    [SerializeField] private Transform startPosition; //Minimap上のstartPositionとendPositionを揃えないと向きが逆になるので注意
    [SerializeField] private Transform endPosition;

    public string CurrentPlaceName
    {
        get { return currentPlaceName; }
        set { currentPlaceName = value; }
    }

    public float GetProgressOnMap()
    {
        Vector3 position = playerObject.transform.position;
        //内分点を探す
        return Mathf.Clamp((position.x - startPosition.position.x) / (endPosition.position.x - startPosition.position.x), 0, 1);
    }

}

