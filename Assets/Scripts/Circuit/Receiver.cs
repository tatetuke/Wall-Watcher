using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]//Connectorがコライダーと接触したかどうか判定するため必要
/// <summary>
/// Connectorと接触できるかどうか判定する
/// </summary>
public class Receiver : MonoBehaviour
{
    public List<string> key;
    [ReadOnly]public bool isConnecting = false;
}
