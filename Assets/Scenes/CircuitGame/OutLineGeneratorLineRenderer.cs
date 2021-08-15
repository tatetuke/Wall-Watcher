using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OutLineGeneratorLineRenderer : MonoBehaviour
{
    LineRenderer line;
    LineRenderer child;
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        var obj = new GameObject("outline");
        child = obj.AddComponent<LineRenderer>();
        child.startWidth = width;
        child.endWidth = width;
        Vector3[] positions=new Vector3[line.positionCount];
        line.GetPositions(positions);
        child.SetPositions(positions);
        child.startColor = lineColor;
        child.endColor = lineColor;
        child.transform.SetParent(transform);
        child.transform.localPosition = Vector3.zero;
        child.material = outlineMaterial;
        child.useWorldSpace = false;
        child.sortingOrder = line.sortingOrder - 1;
    }

    public float width;
    public Color lineColor;
    public Material outlineMaterial;

    // Update is called once per frame
    void Update()
    {
        child.startWidth = width;
        child.endWidth = width;
        Vector3[] positions = new Vector3[line.positionCount];
        line.GetPositions(positions);
        child.SetPositions(positions);
        child.startColor = lineColor;
        child.endColor = lineColor;
        child.material = outlineMaterial;
    }
}
