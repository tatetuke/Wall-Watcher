using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//https://t-stove-k.hatenablog.com/entry/2018/07/08/011925

public class DraggableUI : MonoBehaviour, IDragHandler
{
    public void OnDrag(PointerEventData data)
    {
        Vector3 TargetPos = Camera.main.ScreenToWorldPoint(data.position);
        TargetPos.z = 0;
        transform.position = TargetPos;
    }
}
