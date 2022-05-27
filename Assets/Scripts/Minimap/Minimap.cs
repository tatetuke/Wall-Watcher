using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// 画面に表示される階層ごとのマップUI
/// </summary>
public class Minimap : MonoBehaviour
{
    // プレイヤーの現在位置アイコン
    [SerializeField] private GameObject playerIcon;
    // 今表示されているミニマップ
    [SerializeField,ReadOnly] private GameObject currentPlaceInfoParentObject;
    // ミニマップのprefab
    [SerializeField] private List<GameObject> placeInfoParentPrefabs; 

    // 
    [SerializeField, ReadOnly] private int currentFloor;
    [SerializeField, ReadOnly] private string currentPlaceName;

    private Dictionary<string, PlaceInfo> placeInfoDictionary = new Dictionary<string, PlaceInfo>();
    private Dictionary<string, MinimapIcon> minimapIconDictionary = new Dictionary<string, MinimapIcon>();


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
        //階が変わったらミニマップを変える
        if (currentFloor != LocationGetter.Instance.CurrentFloor)
        {
            ChangeMinimap(LocationGetter.Instance.CurrentFloor);
        }
        currentPlaceName = LocationGetter.Instance.CurrentPlaceName;

        UpdatePlayerIcon();

    }

    /// <summary>指定したゲームオブジェクト名のミニマップアイコンを表示する</summary>
    public void ShowMinimapIcon(string iconName)
    {
        SetMinimapIconActive(iconName,true);
    }

    /// <summary>指定したゲームオブジェクト名のミニマップアイコンを隠す</summary>
    public void HideMinimapIcon(string iconName)
    {
        SetMinimapIconActive(iconName, false);
    }

    public void SetMinimapIconActive(string iconName, bool value)
    {
        minimapIconDictionary[iconName].gameObject.SetActive(value);
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
        
        // 現在のミニマップを削除
        Destroy(currentPlaceInfoParentObject);

        //　新しい階のミニマップを生成
        currentPlaceInfoParentObject = Instantiate(placeInfoParentPrefabs[floor - 1], transform);
        currentPlaceInfoParentObject.transform.SetAsLastSibling();
        playerIcon.transform.SetAsLastSibling();

        //ミニマップの情報を更新
        SetPlaceInfoDictionary();
        SetMinimapIconDictionary();
    }

    /// <summary>placeInfoDictionaryの中身を取得しなおす</summary>
    private void SetPlaceInfoDictionary()
    {
        placeInfoDictionary.Clear();

        // currentPlaceInfoParentの子供のオブジェクトを辞書に追加
        foreach (Transform childTransform in currentPlaceInfoParentObject.transform)
        {
            var scenePlaceInfo = childTransform.GetComponent<PlaceInfo>();
            if (scenePlaceInfo != null)
                //場所の名前(ゲームオブジェクト名)をKeyで登録する
                placeInfoDictionary.Add(scenePlaceInfo.PlaceName, scenePlaceInfo);
        }
    }

    /// <summary>minimapIconDictionaryの中身を取得しなおす</summary>
    private void SetMinimapIconDictionary()
    {
        minimapIconDictionary.Clear();

        // currentPlaceInfoParentの子供のオブジェクトを辞書に追加
        foreach (Transform childTransform in currentPlaceInfoParentObject.transform)
        {
            var minimapIcon = childTransform.GetComponent<MinimapIcon>();
            if (minimapIcon != null)
                //アイコンの名前(ゲームオブジェクト名)をKeyで登録する
                minimapIconDictionary.Add(minimapIcon.IconName, minimapIcon);
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


    [Button]
    public void Hide()
    {
        HideMinimapIcon("Image");
        ShowMinimapIcon("Image (1)");
    }

    [Button]
    public void Show()
    {
        ShowMinimapIcon("Image");
        HideMinimapIcon("Image (1)");
    }
}
