using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 画面に表示される階層ごとのマップUI
/// </summary>
public class Minimap : MonoBehaviour
{
    [SerializeField]private GameObject scenePlaceInfoParent;
    private Dictionary<string, ScenePlaceInfo> scenePlaceInfoDictionary;

    private void Awake()
    {
        //scenePlaceInfoParentの子供のオブジェクトを辞書に追加
        scenePlaceInfoDictionary.Clear();
        foreach(Transform childTransform in scenePlaceInfoParent.transform)
        {
            var scenePlaceInfo = childTransform.GetComponent<ScenePlaceInfo>();
            if (scenePlaceInfo != null)
                scenePlaceInfoDictionary.Add(childTransform.name, scenePlaceInfo);
        }
    }
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
