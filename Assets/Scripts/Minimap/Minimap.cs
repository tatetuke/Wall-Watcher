using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 画面に表示される階層ごとのマップUI
/// </summary>
public class Minimap : MonoBehaviour
{
    [SerializeField] private GameObject playerIcon;
    [SerializeField] private GameObject scenePlaceInfoParent;
    
    [SerializeField, ReadOnly] private string currentPlaceName;

    private Dictionary<string, PlaceInfo> scenePlaceInfoDictionary = new Dictionary<string, PlaceInfo>();

    private void Awake()
    {
        //scenePlaceInfoParentの子供のオブジェクトを辞書に追加
        scenePlaceInfoDictionary.Clear();
        foreach(Transform childTransform in scenePlaceInfoParent.transform)
        {
            var scenePlaceInfo = childTransform.GetComponent<PlaceInfo>();
            if (scenePlaceInfo != null)
                scenePlaceInfoDictionary.Add(childTransform.name, scenePlaceInfo);
        }
    }
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    float t = 0;
    // Update is called once per frame
    private void Update()
    {
        currentPlaceName = LocationGetter.Instance.CurrentPlaceName;

        if (!scenePlaceInfoDictionary.ContainsKey(currentPlaceName))
        {
            Debug.LogWarning("the sceneName is not found");
            return;
        }
        var scenePlaceInfo = scenePlaceInfoDictionary[currentPlaceName];
        //t += Time.deltaTime;
        //playerIcon.transform.position = scenePlaceInfo.GetPlayerPositionOnMinimap(Mathf.Abs(Mathf.Sin(t)));

        //プレイヤーの現在位置からマップ上での進み具合(0~1)を取得
        float progress = LocationGetter.Instance.GetProgressOnMap();
        //Minimap上のアイコンの座標を移動
        playerIcon.transform.position = scenePlaceInfo.GetPlayerPositionOnMinimap(progress);
    }
}
