using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 機械の修理ゲームを定義するScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "NewCircuitGame", menuName = "ScriptableObject/CircuitGame")]
public class CircuitGameSO : ScriptableObject
{
    //この辺は使わないかも
    [Tooltip("修理クエストの名前")]
    public string questName;
    [Tooltip("修理クエストの説明")]
    public string description;

    [Tooltip("修理に必要なパーツ")]
    public List<CircuitSO> requiredCircuit = new List<CircuitSO>();

    

}
