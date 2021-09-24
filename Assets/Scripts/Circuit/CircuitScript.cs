using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Draggable))]
public class CircuitScript : MonoBehaviour
{
    SpriteRenderer sprite;
    Draggable draggable;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        draggable = GetComponent<Draggable>();
    }

    public void SetData(CircuitSO data)
    {
        sprite.sprite = data.icon;
    }

    private void OnMouseOver()
    {
        GetComponent<OutLineGenerator>().width = 0.1f;
    }
    private void OnMouseExit()
    {
        GetComponent<OutLineGenerator>().width = 0.0f;
    }

}
