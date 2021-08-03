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
    public void OnDrag(PointerEventData data)
    {
        transform.position = data.position;
    }
}
