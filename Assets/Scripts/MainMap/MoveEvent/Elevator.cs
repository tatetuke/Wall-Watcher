using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class Elevator : MonoBehaviour
{
    private bool move = false;
    private bool key = false;
    private GameObject elevator;
    private float elapsedTime = 0;

    // マップ移動用
    private bool movekey = false;
    private bool movekey2 = false;
    public int fromNum = 0;
    public int toNum = 0;
    public int autoDirection = 0;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!key && Input.GetKey(KeyCode.Space))
            {
                key = false;
                move = true;
                GameObject camera = GameObject.Find("ChinemachineVirtualCamera");
                camera.GetComponent<CinemachineVirtualCamera>().Follow = null;
            }
        }
    }

    void Start()
    {
        elevator = GameObject.Find("elevator2");
    }

    void Update()
    {
        // エレベータの移動
        if (move)
        {
            if (!movekey2 && elapsedTime >= 2.0)
            {
                movekey2 = true; // 多重ループを回避するための処理
                movekey = true;  // 2秒後にシーン移動処理の開始
            }
            elapsedTime += Time.deltaTime;
            Vector3 tmp = elevator.transform.position;
            elevator.transform.position = new Vector3(tmp.x, tmp.y + (float)0.01, tmp.z);
        }
        // シーンの移動
        if (movekey)
        {
            movekey = false;  // 多重ループを回避するための処理
            AllMapSet.prevMap = fromNum;
            AllMapSet.currentMap = toNum;
            AllMapSet.autoWalkingDirection = autoDirection;
            FadeManager.Instance.LoadLevel(AllMapSet.warpMap[fromNum, toNum].Item3, 1f);
        }
    }
}
