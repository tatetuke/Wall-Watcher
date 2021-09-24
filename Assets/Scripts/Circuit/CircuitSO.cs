using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回路パーツを定義するScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "NewCircuit", menuName = "ScriptableObject/Circuit")]
public class CircuitSO : ItemSO
{
    public GameObject prefab;
}
