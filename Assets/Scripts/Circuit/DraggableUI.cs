using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//https://t-stove-k.hatenablog.com/entry/2018/07/08/011925

/// <summary>
/// DraggableのUI(Canvas)バージョン
/// </summary>
public class DraggableUI : MonoBehaviour, IDragHandler
{
    [SerializeField] Canvas canvas;
    public void OnDrag(PointerEventData data)
    {
       // transform.position = data.position;
    }
    void Update()
    {
        Vector2 MousePos;
        //https://qiita.com/tadayasu61/items/21c1d9f9c1101136dbe3
        /*
         * CanvasのRectTransform内にあるマウスの位置をRectTransformのローカルポジションに変換する
         * canvas.worldCameraはカメラ
         * 出力先はMousePos
         */
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                Input.mousePosition, canvas.worldCamera, out MousePos);

        var r = transform as RectTransform;
        r.anchoredPosition = MousePos;
    }
}
