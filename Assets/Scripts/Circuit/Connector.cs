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
    [ReadOnly,SerializeField]Receiver currentReceiver;
    public string key;//同じkeyをもつReceiverと接続できる
    public bool ignoreKey=true;
    public GameObject connectEffect;
    public class OnConnectorEvent : UnityEvent<Receiver> { }
    public OnConnectorEvent OnConnectEnter = new OnConnectorEvent();
    public OnConnectorEvent OnConnectExit = new OnConnectorEvent();

    public bool Linked() { return currentReceiver != null; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("enter trriger");
        if (collision.gameObject.TryGetComponent<Receiver>(out currentReceiver))
        {
            Debug.Log("connector key:"+currentReceiver.key);
            if (ignoreKey || currentReceiver.key.Contains(key))
            {
                OnConnectEnter.Invoke(currentReceiver);
                currentReceiver.isConnecting = true;
                Instantiate(connectEffect,transform.position,Quaternion.identity).SetActive(true);
            }
            else
            {
                currentReceiver = null;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("exit trriger");
        if (currentReceiver == null) return;
        if (ignoreKey || currentReceiver.key.Contains(key))
        {
            OnConnectExit.Invoke(currentReceiver);
            currentReceiver = null;
        }
    }
}
