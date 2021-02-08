using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkable : MonoBehaviour
{

    [System.Serializable] public class CheckableEvent : UnityEvent<Checker> { }

    public bool m_DestroyAfterChecked = false;
    public bool m_MoveAfterChecked = false;
    [SerializeField] bool m_Gather = false;
    public bool CanGather { get => m_Gather; }

    [SerializeField]
    public CheckableEvent OnChecked = new CheckableEvent();

    public void TakeCheck(Checker checker)
    {
        OnChecked.Invoke(checker);
        if (m_DestroyAfterChecked)
            Destroy(gameObject);
    }

}
