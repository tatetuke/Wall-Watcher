using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 画面に表示される階層ごとのマップUI
/// </summary>
public class Minimap : MonoBehaviour
{
    [SerializeField] private GameObject playerIcon;
    [SerializeField,ReadOnly] private GameObject scenePlaceInfoParent;

    [SerializeField] private List<GameObject> placeInfoParentPrefabs;

    [SerializeField, ReadOnly] private int currentFloor;
    [SerializeField, ReadOnly] private string currentPlaceName;

    private Dictionary<string, PlaceInfo> placeInfoDictionary = new Dictionary<string, PlaceInfo>();

    

    private void Awake()
    {

    }
    // Start is called before the first frame update
    private void Start()
    {
        ChangeMinimap(LocationGetter.Instance.CurrentFloor);
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentFloor != LocationGetter.Instance.CurrentFloor)
        {
            ChangeMinimap(LocationGetter.Instance.CurrentFloor);
        }
        currentPlaceName = LocationGetter.Instance.CurrentPlaceName;

        UpdatePlayerIcon();

    }

    /// <summary>指定した階層のマップに切り替える</summary>
    /// <param name="floor">1~5</param>
    public void ChangeMinimap(int floor)
    {
        if (currentFloor == floor)
            return;
        currentFloor = floor;
        if(floor < 1 || floor > 5)
        {
            Debug.LogWarning("\"floor\" is out of range");
            return;
        }

        GameObject mapObj = Instantiate(placeInfoParentPrefabs[floor - 1], transform);
        mapObj.transform.SetAsLastSibling();
        playerIcon.transform.SetAsLastSibling();

        Destroy(scenePlaceInfoParent);
        scenePlaceInfoParent = mapObj;

        SetPlaceInfoDictionary();
    }

    /// <summary>placeInfoDictionaryの中身を取得しなおす</summary>
    private void SetPlaceInfoDictionary()
    {
        //scenePlaceInfoParentの子供のオブジェクトを辞書に追加
        placeInfoDictionary.Clear();
        foreach (Transform childTransform in scenePlaceInfoParent.transform)
        {
            var scenePlaceInfo = childTransform.GetComponent<PlaceInfo>();
            if (scenePlaceInfo != null)
                placeInfoDictionary.Add(childTransform.name, scenePlaceInfo);
        }
    }
    
    /// <summary>Minimap上のプレイヤーのアイコンの位置を更新する</summary>
    private void UpdatePlayerIcon()
    {
        if (!placeInfoDictionary.ContainsKey(currentPlaceName))
        {
            Debug.LogWarning("the sceneName is not found");
            return;
        }
        var scenePlaceInfo = placeInfoDictionary[currentPlaceName];

        //プレイヤーの現在位置からマップ上での進み具合(0~1)を取得
        float progress = LocationGetter.Instance.GetProgressOnMap();
        //Minimap上のアイコンの座標を移動
        playerIcon.transform.position = scenePlaceInfo.GetPlayerPositionOnMinimap(progress);
    }

}
