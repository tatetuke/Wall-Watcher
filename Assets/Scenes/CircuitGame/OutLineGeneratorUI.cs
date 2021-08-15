using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 回路で使う輪郭を表示するためのスクリプト
/// シェーダーでやってもよかったのだが、回路のシェーダーがもうすでにデフォルトのものでないし、
/// シェーダーのだと太く輪郭を表示するのがうまくいかないので
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Image))]
public class OutLineGeneratorUI : MonoBehaviour
{
    public float width;
    [ColorUsage(true, true)]
    public Color color;
    public Material outlineMaterial;

    Image image;

    List<Image> child = new List<Image>();

    private void Awake()
    {
        image = GetComponent<Image>();
        //輪郭線となるオブジェクトを子として生成する
        for (int i = 0; i < 8; i++)
        {
            var obj = new GameObject();
            var sp = obj.AddComponent<Image>();
            sp.material = outlineMaterial;
            sp.sprite = image.sprite;
            sp.preserveAspect = true;
            sp.maskable = false;
            sp.raycastTarget = false;
            obj.transform.SetParent(transform);
            obj.transform.localScale = Vector3.one;
            sp.rectTransform.sizeDelta = image.rectTransform.sizeDelta;
            child.Add(sp);
        }

        //もとの画像が子オブジェクトに上書きされてしまうので、
        //もとの画像として表示するものを子オブジェクトとして生成
        var orig_obj = new GameObject();
        var orig_sp = orig_obj.AddComponent<Image>();
        orig_sp.material = image.material;
        orig_sp.sprite = image.sprite;
        orig_sp.preserveAspect = true;
        orig_sp.maskable = false;
        orig_sp.raycastTarget = false;
        orig_obj.transform.SetParent(transform);
        orig_obj.transform.localScale = Vector3.one;
        orig_sp.rectTransform.sizeDelta = image.rectTransform.sizeDelta;
        child.Add(orig_sp);
        update_child();
    }

    void update_child()
    {
        if (child.Count < 8) return;
        float sqr2_inv = 1f / Mathf.Sqrt(2);
        child[0].transform.position = transform.position + new Vector3(1, 0) * width;
        child[1].transform.position = transform.position + new Vector3(-1, 0) * width;
        child[2].transform.position = transform.position + new Vector3(0, 1) * width;
        child[3].transform.position = transform.position + new Vector3(0, -1) * width;
        child[4].transform.position = transform.position + new Vector3(1, 1) * width * sqr2_inv;
        child[5].transform.position = transform.position + new Vector3(-1, 1) * width * sqr2_inv;
        child[6].transform.position = transform.position + new Vector3(1, -1) * width * sqr2_inv;
        child[7].transform.position = transform.position + new Vector3(-1, -1) * width * sqr2_inv;
        for (int i = 0; i < 8; i++)
        {
            child[i].material.SetColor("_MainColor", color);
            child[i].rectTransform.sizeDelta = image.rectTransform.sizeDelta;
        }
        child[8].transform.position = transform.position;
        child[8].rectTransform.sizeDelta = image.rectTransform.sizeDelta;
        child[8].color = image.color;
    }
    private void Update()
    {
        update_child();
    }
    //輪郭より上に表示する画像を取得
    public Image GetOriginalImage()
    {
        return child[8];
    }

    public void ChangeImage(Sprite sprite)
    {
        for (int i = 0; i < 9; i++)
            child[i].sprite = sprite;
        update_child();
    }
}
