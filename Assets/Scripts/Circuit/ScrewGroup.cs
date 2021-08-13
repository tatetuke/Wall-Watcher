using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class ScrewGroup : MonoBehaviour
{
    List<Screw> children = new List<Screw>();
    //オブジェクトにおおわれて動かせなくなっている状態のDraggableを登録
    [SerializeField] List<Draggable> coveredDraggable = new List<Draggable>();
    [SerializeField] string animationAllRemoved;
    Animator animator;
    public UnityEvent OnAllScrewRemoved { get; } = new UnityEvent();

    int removedCount = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        children.AddRange(GetComponentsInChildren<Screw>());
    }
    private void Start()
    {
        foreach (var i in children)
        {
            if (i == null) continue;
            i.OnRemoved.AddListener(() =>
            {
                removedCount++;
                if (AllScrewRemoved())
                {
                    OnAllScrewRemoved.Invoke();
                }
            });
        }
        OnAllScrewRemoved.AddListener(() =>
        {
            foreach (var i in coveredDraggable)
            {
                if (i == null) continue;
                i.canDrag = true;
            }
            animator.Play(animationAllRemoved);
        });
        foreach (var i in coveredDraggable)
        {
            if (i == null) continue;
            i.canDrag = false;
        }
    }


    public bool AllScrewRemoved()
    {
        return removedCount == children.Count;
    }

}
