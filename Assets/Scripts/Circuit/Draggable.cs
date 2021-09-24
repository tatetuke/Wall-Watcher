using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

//https://t-stove-k.hatenablog.com/entry/2018/07/08/011925
/// <summary>
/// マウスでドラッグできる
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool canDrag = true;
    public bool resetOnReceiverNotFound = false;//Receiverにドラッグできなかったら座標を戻す

    public class OnDragEvent : UnityEvent<Vector3> { }
    public OnDragEvent OnDragging = new OnDragEvent();
    public OnDragEvent OnSetPosition = new OnDragEvent();
    public UnityEvent OnDragBegin = new UnityEvent();
    public UnityEvent OnDragEnd = new UnityEvent();

    Vector3 initialPos;
    Vector3 posDelta;


    // ドラックが開始したとき呼ばれる.
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        Vector3 TargetPos = Camera.main.ScreenToWorldPoint(eventData.position);
        posDelta = transform.position - TargetPos;
        initialPos = transform.position;
        OnDragBegin.Invoke();
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;
        Vector3 TargetPos = Camera.main.ScreenToWorldPoint(eventData.position);
        TargetPos.z = 0;
        transform.position = TargetPos + posDelta;
        OnDragging.Invoke(transform.position);
    }
    // ドラックが終了したとき呼ばれる.
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        OnDragEnd.Invoke();
        /*if (resetOnReceiverNotFound)
        {
            transform.position = initialPos;
        }*/
    }
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        OnSetPosition.Invoke(position);
    }
}
