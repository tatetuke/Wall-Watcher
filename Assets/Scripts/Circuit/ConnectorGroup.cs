using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Draggable))]
[DisallowMultipleComponent]
/// <summary>
/// このgameobjectの子についているConnectorがすべてReceiverと接触しないとくっついたことにならない
/// </summary>
public class ConnectorGroup : MonoBehaviour
{
    Connector[] m_connectors;
    Draggable m_draggable;
    [ReadOnly,SerializeField]int connectorLength = 0;//どれだけのCOnnectorが子として存在するか
    [ReadOnly,SerializeField]int currentCounter = 0;//現在どれだけのCOnnectorが接触しているか
    // Start is called before the first frame update
    void Start()
    {
        m_draggable = GetComponent<Draggable>();
        m_connectors =GetComponentsInChildren<Connector>();
        connectorLength = m_connectors.Length;
        foreach(var connector in m_connectors)
        {
            connector.OnConnectEnter.AddListener((receiver)=>{
                currentCounter++;
                if (currentCounter == connectorLength)
                {
                    m_draggable.canDrag = false;
                }
                connector.transform.position = receiver.transform.position;
            });
            connector.OnConnectExit.AddListener((receiver) => {
                currentCounter--;
                m_draggable.canDrag = true;
            });
        }
    }
}
