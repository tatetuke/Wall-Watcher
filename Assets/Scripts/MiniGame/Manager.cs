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
    int[,] Wall;
    int Num = 0;
    GameObject Item;

    public GameObject Item1, Item2;

    public void Start()
    {

    }


    public void Update()
    {
        Num = 0;
        
        GameObject target;
        target = GetClickObject();
        if (target != null) Debug.Log(target.name);

        if (target == null) return;
        else if (target.name == "Green")
        {
            Instantiate(Item1);
            Item = GameObject.Find("Item1(Clone)").gameObject;
        }
        else if (target.name == "Blue")
        {
            Item = GameObject.Find("Item1(Clone)");
            if (Item != null) Destroy(Item);

            Item = GameObject.Find("Item2(Clone)");
            if (Item == null) Instantiate(Item1);
            Item = GameObject.Find("Item2(Clone)");
        }
        else if (target.name == "White")
        {
            Item = GameObject.Find("Item1(Clone)");
            if (Item != null) Destroy(Item);
            Item = GameObject.Find("Item2(Clone)");
            if (Item != null) Destroy(Item);
            Item = null;
        }
        else
        {
            Wall wall;
            wall = target.GetComponent<Wall>();
            wall.ChangeSprite(Num);
        }

        if (Item != null)
        {
            Debug.Log("更新");
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
            Num = +1;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                result = hit.collider.gameObject;
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Num = -1;
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
