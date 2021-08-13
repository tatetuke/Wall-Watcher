using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回路のMaterialを操作するスクリプト
/// </summary>

[RequireComponent(typeof(Renderer))]
[DisallowMultipleComponent]
public class CurcuitMaterialEditor : MonoBehaviour
{
    [SerializeField] string KEY = "thre";
    [SerializeField, ReadOnly] Material material;
    [SerializeField,ReadOnly]float current_t;
    [SerializeField] Connector target;
    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        if (!material.HasProperty(KEY))
        {
            Debug.LogError($"material does not have {KEY}");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SetT(1);//電線はOffの状態
        target.OnConnectEnter.AddListener((receiver)=> {
            Debug.Log("material enter");
            DG.Tweening.DOTween.To(() => current_t, (value) => SetT(value), 0f,1f);
        });
        target.OnConnectExit.AddListener((receiver) => {
            Debug.Log("material exit");
            DG.Tweening.DOTween.To(() => current_t, (value) => SetT(value), 1f, 1f);
        });
    }

    /// <summary>
    /// 回路の電線のOn/Offのアニメーション
    /// </summary>
    /// <param name="t"></param>
    public void SetT(float t)
    {
        current_t = t;
        material.SetFloat(KEY, current_t);
    }
    public void SetEmission(Sprite sprite)
    {
        material.SetTexture("EmisssionTexture", sprite.texture);
    }

}
