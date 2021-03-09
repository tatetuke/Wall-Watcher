using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ミニゲームの内容自体を管理する
/// </summary>
public class Manager : MonoBehaviour
{
    // 壁の状態
    GameObject Item;

    public GameObject Item1, Item2;

    public void Start()
    {

    }


    public void Update()
    {

        GameObject target;
        target = GetClickObject();

        if (target != null)
        {
            if (target.name == "Green")
            {
                if (Item == null)
                {
                    Instantiate(Item1);
                    Item = GameObject.Find("Item1(Clone)");
                }
                else if (Item.name != "Item1(Clone)")
                {
                    Destroy(Item);
                    Instantiate(Item1);
                    Item = GameObject.Find("Item1(Clone)");
                }
            }
            else if (target.name == "Blue")
            {
                if (Item == null)
                {
                    Instantiate(Item1);
                    Item = GameObject.Find("Item2(Clone)");
                }
                else if (Item.name != "Item2(Clone)")
                {
                    Destroy(Item);
                    Instantiate(Item2);
                    Item = GameObject.Find("Item2(Clone)");
                }
            }
            else if (target.name == "White")
            {
                if (Item != null) Destroy(Item);
            }
            else
            {
                Wall wall;
                wall = target.GetComponent<Wall>();

                if (Item != null)
                {
                    if (Item.name == "Item1(Clone)")
                    {
                        if (wall.GetState() == Wall.WallState.DRY)
                            wall.ChangeSprite(Wall.WallState.PAINTED);
                    }
                    else if (Item.name == "Item2(Clone)")
                    {
                        if (wall.GetState() == Wall.WallState.CRACKED)
                            wall.ChangeSprite(Wall.WallState.DRY);
                    }
                }
            }
        }

        if (Item != null)
        {
            Vector3 touchScreenPosition = Input.mousePosition;

            touchScreenPosition.x = Mathf.Clamp(touchScreenPosition.x, 0.0f, Screen.width);
            touchScreenPosition.y = Mathf.Clamp(touchScreenPosition.y, 0.0f, Screen.height);

            // 10.0fに深い意味は無い。画面に表示したいので適当な値を入れてカメラから離そうとしているだけ.
            touchScreenPosition.z = 10.0f;

            Camera gameCamera = Camera.main;
            Vector3 touchWorldPosition = gameCamera.ScreenToWorldPoint(touchScreenPosition);

            Item.transform.position = touchWorldPosition;
        }
    }



    private GameObject GetClickObject()
    {
        GameObject result = null;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                result = hit.collider.gameObject;
            }
        }
        return result;
    }
}
