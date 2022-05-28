using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

// 外部からは
// ShowMinimapIcon(string iconName)
// HideMinimapIcon(string iconName)
// を使う想定

/// <summary>
/// 画面に表示されるマップUIを管理する
/// </summary>
public class Minimap : SingletonMonoBehaviour<Minimap>
{
    // プレイヤーの現在位置アイコン
    [SerializeField] private GameObject playerIcon;
    // ミニマップが表示されるパネル
    [SerializeField] private GameObject minimapPanel;
    // 今表示されているミニマップ
    [SerializeField,ReadOnly] private GameObject currentPlaceInfoParentObject;
    // ミニマップのprefab
    [SerializeField] private List<GameObject> placeInfoParentPrefabs; 

    // 今の階
    [SerializeField, ReadOnly] private int currentFloor;
    // 今の場所の名前
    [SerializeField, ReadOnly] private string currentPlaceName;

    // 現在表示されているミニマップに含まれる場所の辞書
    private Dictionary<string, PlaceInfo> placeInfoDictionary = new Dictionary<string, PlaceInfo>();
    // 現在表示されているミニマップに含まれるミニマップアイコンの辞書
    private Dictionary<string, MinimapIcon> minimapIconDictionary = new Dictionary<string, MinimapIcon>();


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

    /// <summary>
    /// 指定した名前を表示リストに加え、指定した名前のミニマップアイコンの表示・非表示にする
    /// </summary>
    /// <param name="iconName"></param>
    /// <param name="value"></param>
    public void SetMinimapIconActive(string iconName, bool value)
    {
        // 表示リストに追加・削除
        MinimapIconFlagHolder.Instance.OperateVisibleMinimapIcon(iconName, value);
        
        MinimapIcon minimapIcon;
        // 今のミニマップに存在する場合は表示、または非表示に切り替える
        if(minimapIconDictionary.TryGetValue(iconName, out minimapIcon))
        {
            minimapIconDictionary[iconName].gameObject.SetActive(value);
        }
        // 存在しない場合は何もしない
        
    }

    public void HideAllMinimapIcon()
    {
        foreach(var pair in minimapIconDictionary)
        {
            HideMinimapIcon(pair.Key);
        }
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
        //currentPlaceInfoParentObject = Instantiate(placeInfoParentPrefabs[floor - 1], minimapPanel.transform);
        
        //描画順を変更
        currentPlaceInfoParentObject.transform.SetAsLastSibling();
        playerIcon.transform.SetAsLastSibling();

        //ミニマップの情報から辞書を更新
        SetPlaceInfoDictionary();
        SetMinimapIconDictionary();

        // MinimapIconFlagHolder に存在するミニマップアイコンを表示する
        LoadMinimapIconFlagHolder();
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
            {
                // アイコンの名前(ゲームオブジェクト名)をKeyで登録する
                minimapIconDictionary.Add(minimapIcon.IconName, minimapIcon);
                // 非表示で初期化する
                minimapIconDictionary[minimapIcon.IconName].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// MinimapIconFlagHolder に保存されているアイコンを可視化する
    /// </summary>
    private void LoadMinimapIconFlagHolder()
    {
        // MinimapIconFlagHolder に登録されていたら可視化する
        foreach (var pair in minimapIconDictionary)
        {
            string iconName = pair.Key;
            if (MinimapIconFlagHolder.Instance.ContainsKey(iconName))
            {
                SetMinimapIconActive(iconName, true);
            }
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
    
    [Button]
    public void Load()
    {
        LoadMinimapIconFlagHolder();
    }
}
