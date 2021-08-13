using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(EventTrigger))]
[RequireComponent(typeof(Animator))]
public class Screw : MonoBehaviour
{
    [SerializeField] string removing;
    public UnityEvent OnRemoved { get; } = new UnityEvent();

    Animator animation;
    private void Awake()
    {
        animation = GetComponent<Animator>();
    }
    public void OnClick()
    {
        Debug.Log("clicked");
        animation.Play(removing);
        OnRemoved.Invoke();
    }
}
