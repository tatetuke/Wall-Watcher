using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Picker : MonoBehaviour
{
    [SerializeField] Collider2D pickCollider;
    [SerializeField] Collider2D gatherCollider;
    public ContactFilter2D colliderFilter;
    List<Pickable> currentGather = new List<Pickable>();
    [System.Serializable]
    public class OnPickUpEvent : UnityEvent<Picker, Pickable> { }
    public OnPickUpEvent OnPickUp { get; } = new OnPickUpEvent();

    // Update is called once per frame
    void Update()
    {
        if (gatherCollider != null)
        {
            Collider2D[] gather_result = new Collider2D[32];
            Physics2D.OverlapCollider(gatherCollider, colliderFilter, gather_result);
            foreach (var obj in gather_result)
            {
            if (obj == null) continue;
                Pickable component;
                if (obj.TryGetComponent<Pickable>(out component))
                {
                    component.GatherTo(this);
                }
            }
        }
        Collider2D[] get_result = new Collider2D[32];
        Physics2D.OverlapCollider(pickCollider, colliderFilter, get_result);
        foreach (var obj in get_result)
        {
            if (obj == null) continue;
            Pickable component;
            if (obj.TryGetComponent<Pickable>(out component))
            {
                OnPickUp.Invoke(this, component);
                component.PickUp(this);
            }
        }
    }

}
