using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
[DisallowMultipleComponent]
public class CircuitItemUI : MonoBehaviour
{
    public UnityEvent OnPointerDown { get; } = new UnityEvent();
    public UnityEvent OnPointerUp { get; } = new UnityEvent();

    private void Start()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { OnPointerDown.Invoke(); });
        GetComponent<EventTrigger>().triggers.Add(entry);
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerUp;
        entry2.callback.AddListener((data) => { OnPointerUp.Invoke(); });
        GetComponent<EventTrigger>().triggers.Add(entry2);
    }

    public void SetImage(Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
    }

   
}
