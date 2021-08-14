using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Draggable))]
[RequireComponent(typeof(CurcuitMaterialEditor))]
public class CircuitScript : MonoBehaviour
{
    SpriteRenderer sprite;
    Draggable draggable;
    CurcuitMaterialEditor materialEditor;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        draggable = GetComponent<Draggable>();
        materialEditor = GetComponent<CurcuitMaterialEditor>();
    }

    public void SetData(CircuitSO data)
    {
        sprite.sprite = data.icon;
        materialEditor.SetEmission(data.emission);
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
