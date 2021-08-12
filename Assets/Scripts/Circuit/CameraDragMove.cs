using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//参考（ではない）
//https://robamemo.hatenablog.com/entry/2018/11/22/200458


[DisallowMultipleComponent]
public class CameraDragMove : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Camera camera;
    /// <summary>
    /// カメラの移動感度
    /// </summary>
    public float sensitivity;
    // Start is called before the first frame update
    void Start()
    {
        camera = transform.root.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    Vector2 beginPos;
    Vector3 cameraBeginPos;
    // ドラックが開始したとき呼ばれる.
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("camera drag begin");
        beginPos = eventData.position;
        cameraBeginPos = camera.transform.position;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - beginPos;
        camera.transform.position = cameraBeginPos - (Vector3)delta* sensitivity;
    }

    // ドラックが終了したとき呼ばれる.
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("camera drag end");

    }

}
