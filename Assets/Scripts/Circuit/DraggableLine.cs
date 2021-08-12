using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DraggableLine : MonoBehaviour
{
    public string key;
    public GameObject connectEffect;
    [SerializeField] Connector defaultConnect;
    LineRenderer m_line;
    List<Draggable> childDraggable = new List<Draggable>();
    private void Start()
    {
        m_line = GetComponent<LineRenderer>();
        for (int i = 0; i < m_line.positionCount; i++)
        {
            GameObject obj = new GameObject();
            obj.transform.parent = transform;
            int index = i;
            obj.transform.position = m_line.GetPosition(index) + m_line.transform.position;
            var collider = obj.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(m_line.startWidth * 4, m_line.startWidth * 4);
            var scr = obj.AddComponent<Draggable>();
            scr.OnDragging.AddListener((pos) =>
            {
                m_line.SetPosition(index, pos - m_line.transform.position);
            });
            scr.OnSetPosition.AddListener((pos) =>
            {
                m_line.SetPosition(index, pos - m_line.transform.position);
            });
            var connector = obj.AddComponent<Connector>();
            connector.connectEffect = connectEffect;
            connector.key = key;
            connector.ignoreKey = false;
            childDraggable.Add(scr);

            var connectorGroup = obj.AddComponent<ConnectorGroup>();
        }
    }
}
