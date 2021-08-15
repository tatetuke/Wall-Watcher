using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(OutLineGeneratorUI))]
[RequireComponent(typeof(EventTrigger))]
[DisallowMultipleComponent]
public class CircuitItemUI : MonoBehaviour
{
    public UnityEvent OnPointerDown { get; } = new UnityEvent();
    public UnityEvent OnPointerUp { get; } = new UnityEvent();
    OutLineGeneratorUI outline;

    private void Awake()
    {
        outline = GetComponent<OutLineGeneratorUI>();

    }

    private void Start()
    {
        var eventTrigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { OnPointerDown.Invoke(); });
        eventTrigger.triggers.Add(entry);
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerUp;
        entry2.callback.AddListener((data) => { OnPointerUp.Invoke(); });
        eventTrigger.triggers.Add(entry2);


        var button = GetComponent<Button>();
        button.targetGraphic = outline.GetOriginalImage();
        var entry3 = new EventTrigger.Entry();
        //　イベントの種類を設定
        entry3.eventID = EventTriggerType.PointerEnter;
        entry3.callback.AddListener(data => {
            outline.width = 0.1f;
        });
        eventTrigger.triggers.Add(entry3);
        var entry4 = new EventTrigger.Entry();
        //　イベントの種類を設定
        entry4.eventID = EventTriggerType.PointerExit;
        entry4.callback.AddListener(data => {
            outline.width = 0;
        });
        eventTrigger.triggers.Add(entry4);
    }

    public void SetImage(Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
        GetComponent<OutLineGeneratorUI>().ChangeImage(sprite);
    }

   
}
