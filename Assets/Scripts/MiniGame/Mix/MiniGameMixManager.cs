using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameMixManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (/*左クリックが押されたら*/Input.GetMouseButtonDown(0))
        {
            GameObject cursorObject = GetCursorObject();
            if (cursorObject != null)
            {
                if (cursorObject.name == "ww_tyougou_botan1")
                {
                    Debug.Log("↑ボタンが押されました");
                }
                else if (cursorObject.name == "ww_tyougou_botan2")
                {
                    Debug.Log("↓ボタンが押されました");
                }
            }
        }
    }

    private GameObject GetCursorObject()
    {
        GameObject cursorObject;
        cursorObject = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
        if (hit2d)
        {
            cursorObject = hit2d.transform.gameObject;
        }
        return cursorObject;
    }
}
