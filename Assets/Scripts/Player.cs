﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの移動・見た目周りを管理するクラス
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField]
    private float m_WalkSpeed = 0;
    [SerializeField]
    private State m_State = default;
    [SerializeField]
    private GameObject m_ViewObject = default;

    //private CharacterController m_CharacterController = default;

    public enum State
    {
        FREEZE,//移動不可能
        IDEL,//立ち状態
        WALKING,//歩き
        MINIGAME,//ミニゲーム
    }

    // Start is called before the first frame update
    void Start()
    {
        m_State = State.IDEL;
        //m_CharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal") * m_WalkSpeed * Time.deltaTime;
        Vector3 vel = new Vector3(moveX, 0, 0);
        
        //横移動
        transform.position = transform.position + vel;
        //m_CharacterController.SimpleMove(vel);
        //m_CharacterController.Move(vel);

        //移動する向きに見た目を回転
        if (moveX != 0)
        {
            int dir = moveX > 0 ? 180 : 0;
            //transform.rotation = Quaternion.Euler(0, dir, 0);
            m_ViewObject.transform.rotation = Quaternion.Euler(0, dir, 0);
        }

    }

    public void ChangeState(State stete)
    {

    }

    private void UpdateState()
    {
        if (m_State == State.FREEZE)
        {

        }
        else if (m_State == State.IDEL)
        {
            Move();
        }
        else if (m_State == State.WALKING)
        {
            Move();
        }
        else if (m_State == State.MINIGAME)
        {

        }
        else
        {
            Debug.LogWarning("対応できていない状態が存在します");
        }
    }

}
