using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回路で使う輪郭を表示するためのスクリプト
/// シェーダーでやってもよかったのだが、回路のシェーダーがもうすでにデフォルトのものでないし、
/// シェーダーのだと太く輪郭を表示するのがうまくいかないので
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public class OutLineGenerator : MonoBehaviour
{
    public float width;
    [ColorUsage(true, true)]
    public Color color;
    public Material outlineMaterial;

    SpriteRenderer sprite;

    List<SpriteRenderer> child = new List<SpriteRenderer>();

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            var obj = new GameObject();
            var sp = obj.AddComponent<SpriteRenderer>();
            sp.material = outlineMaterial;
            sp.sprite = sprite.sprite;
            obj.transform.SetParent(transform);
            sp.sortingOrder = sprite.sortingOrder - 1;
            child.Add(sp);
           
        }
        update_child();
    }


    void update_child()
    {
        if (child.Count < 8) return;
        child[0].transform.position = transform.position + new Vector3(1, 0) * width;
        child[0].material.SetColor("_MainColor", color);
        child[1].transform.position = transform.position + new Vector3(-1, 0) * width;
        child[1].material.SetColor("_MainColor", color);
        child[2].transform.position = transform.position + new Vector3(0, 1) * width;
        child[2].material.SetColor("_MainColor", color);
        child[3].transform.position = transform.position + new Vector3(0, -1) * width;
        child[3].material.SetColor("_MainColor", color);
        float sqr2_inv = 1f / Mathf.Sqrt(2);
        child[4].transform.position = transform.position + new Vector3(1, 1) * width* sqr2_inv;
        child[4].material.SetColor("_MainColor", color);
        child[5].transform.position = transform.position + new Vector3(-1, 1) * width * sqr2_inv;
        child[5].material.SetColor("_MainColor", color);
        child[6].transform.position = transform.position + new Vector3(1, -1) * width * sqr2_inv;
        child[6].material.SetColor("_MainColor", color);
        child[7].transform.position = transform.position + new Vector3(-1, -1) * width * sqr2_inv;
        child[7].material.SetColor("_MainColor", color);
    }
    private void OnValidate()
    {
        update_child();


    }
}
