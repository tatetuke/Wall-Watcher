using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
/// <summary>
/// Receiverと接触しているかどうか判定する
/// 接触してたらドラッグできないようにする処理はConnectorGroupで行う
/// </summary>
public class Connector : MonoBehaviour
{
    //接触している Connector
    [ReadOnly, SerializeField] Connector currentConnector;
    public string key;//同じkeyをもつReceiverと接続できる
    public bool ignoreKey = true;
    public List<string> targetKeys = new List<string>();
    public GameObject connectEffect;
    bool connecting = false;

    public class OnConnectorEvent : UnityEvent<Connector> { }
    public OnConnectorEvent OnConnectEnter = new OnConnectorEvent();
    public OnConnectorEvent OnConnectExit = new OnConnectorEvent();

    public bool Linked() { return currentConnector != null; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("enter trriger");
        if (collision.gameObject.TryGetComponent<Connector>(out currentConnector))
        {
            if (ignoreKey || currentConnector.CanConnect(key))
            {
                OnConnectEnter.Invoke(currentConnector);
                currentConnector.Connect();
                Instantiate(connectEffect, transform.position, Quaternion.identity).SetActive(true);
                //currentDraggable.stopTillDragEnd = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("exit trriger");
        if (currentConnector == null) return;
        if (collision.GetComponent<Connector>() == null) return;
        if (ignoreKey || currentConnector.CanConnect(key))
        {
            OnConnectExit.Invoke(currentConnector);
            currentConnector.Disconnect();
            currentConnector = null;
        }
    }


    public void Connect()
    {
        connecting = true;
    }
    public void Disconnect()
    {
        connecting = false;
    }

    public bool CanConnect(string key)
    {
        foreach (var i in targetKeys)
            if (i == key) return true;
        return false;
    }
}
