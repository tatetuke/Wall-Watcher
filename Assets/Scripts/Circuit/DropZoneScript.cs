using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DropZoneScript : MonoBehaviour,IDropHandler//, IPointerExitHandler
{
    public class OnDragObjEvent : UnityEvent<Image> { }
    public OnDragObjEvent OnDropObj { get; } = new OnDragObjEvent();

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped",gameObject);
        if (eventData.pointerDrag == null) return;
        if (eventData.pointerDrag.TryGetComponent<Image>(out Image output))
        {
            OnDropObj.Invoke(output);
        }
    }
}
