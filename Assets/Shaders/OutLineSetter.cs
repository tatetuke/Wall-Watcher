using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OutLineシェーダーのパラメータをスクリプトから制御するためのもの
/// </summary>
[RequireComponent(typeof(Renderer))]
public class OutLineSetter : MonoBehaviour
{
    Material m_material;

    private void Awake()
    {
        m_material = GetComponent<Renderer>().material;
    }
    public void SetWidth(float value)
    {
        m_material.SetFloat("outLineWidth",value);
    }
    public void SetOutLineColor(Color color)
    {
        m_material.SetColor("OutLineColor", color);
    }
}
