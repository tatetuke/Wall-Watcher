using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameMixManager : MonoBehaviour
{
    [SerializeField] GameObject ButtonUp;
    [SerializeField] GameObject ButtonDown;
    private bool IsMouseDown = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (/*左クリックが押されている*/Input.GetMouseButton(0))
        {
            GameObject cursorObject = GetCursorObject();
            GlowButton(cursorObject);
        }
        else
            HideGlowBotton();
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

    private void GlowButton(GameObject cursorObject)
    {
        if (cursorObject == null)
            HideGlowBotton();
        else if (cursorObject.name == "ww_tyougou_botan1")
            ButtonUp.SetActive(true);
        else if (cursorObject.name == "ww_tyougou_botan2")
            ButtonDown.SetActive(true);
        else
            HideGlowBotton();
    }

    private void HideGlowBotton()
    {
        ButtonUp.SetActive(false);
        ButtonDown.SetActive(false);
    }
}
