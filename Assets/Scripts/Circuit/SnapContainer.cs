using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
/// <summary>
/// Draggableをスナップできる
/// 特定のスロットに入れたりとか外したり
/// </summary>
public class SnapContainer : MonoBehaviour
{
    [ReadOnly,SerializeField]Draggable currentDraggable;
    //public string key;
    //public bool ignoreKey = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Draggable>(out currentDraggable))
        {
            currentDraggable.SetPosition(transform.position);
            currentDraggable.stopTillDragEnd = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
