using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Pickable : MonoBehaviour
{
     Rigidbody2D m_rigidbody2D;

    // Start is called before the first frame update
    void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void GatherTo(Picker picker)
    {
        var vec = (picker.transform.position - transform.position).normalized;
        m_rigidbody2D.AddForce(vec);
    }

    public void PickUp(Picker picker)
    {
        Destroy(gameObject);
    }

}
