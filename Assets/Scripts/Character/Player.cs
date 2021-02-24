using System.Collections;
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
    private float m_WalkForce = 0;
    [SerializeField]
    private State m_State = default;
    [SerializeField]
    private GameObject m_ViewObject = default;
    [SerializeField]
    private Rigidbody2D m_Rigidbody2D = default;
    [SerializeField]
    private AnimationModule m_AnimationModule = default;

    public enum State
    {
        FREEZE,//移動不可能
        IDLE,//立ち状態
        WALKING,//歩き
        MINIGAME,//ミニゲーム
    }

    public bool IsWalking
    {
        get { return Input.GetAxis("Horizontal") != 0; }
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(State.IDLE);
        //m_CharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateState();
    }

    private void Move()
    {
        //横移動
        //座標を直接いじる
        //float moveX = Input.GetAxis("Horizontal") * m_WalkSpeed * Time.deltaTime;
        //Vector3 vel = new Vector3(moveX, 0, 0);
        //transform.position = transform.position + vel;

        //速度を直接いじる
        //float moveX = Input.GetAxis("Horizontal") * m_WalkSpeed;
        //Vector3 vel = m_Rigidbody2D.velocity;
        //vel.x = moveX;
        //m_Rigidbody2D.velocity = vel;

        //力を加える
        float moveX = Input.GetAxis("Horizontal") * m_WalkForce;
        float moveForceMultiplier = 10;
        Vector3 vel = new Vector3(moveForceMultiplier *( moveX - m_Rigidbody2D.velocity.x), 0, 0);
        m_Rigidbody2D.AddForce(vel);

        //移動する向きに見た目を回転
        if (moveX != 0)
        {
            int dir = moveX > 0 ? 180 : 0;
            //transform.rotation = Quaternion.Euler(0, dir, 0);
            m_ViewObject.transform.rotation = Quaternion.Euler(0, dir, 0);
        }

    }

    public void ChangeState(State state)
    {
        m_State = state;
        if (m_State == State.FREEZE)
        {

        }
        else if (m_State == State.IDLE)
        {
            m_AnimationModule.ChangeState(AnimationModule.State.IDLE);
        }
        else if (m_State == State.WALKING)
        {
            m_AnimationModule.ChangeState(AnimationModule.State.WALKING);
        }
        else if (m_State == State.MINIGAME)
        {

        }
        else
        {
            Debug.LogWarning("対応できていない状態です");
        }
    }

    private void UpdateState()
    {
        if (m_State == State.FREEZE)
        {

        }
        else if (m_State == State.IDLE)
        {
            if (IsWalking)
                ChangeState(State.WALKING);
        }
        else if (m_State == State.WALKING)
        {
            if (!IsWalking)
                ChangeState(State.IDLE);
            Move();
        }
        else if (m_State == State.MINIGAME)
        {

        }
        else
        {
            Debug.LogWarning("対応できていない状態です");
        }
    }

    public void PlayFootStepSE()
    {
        SEManager.Instance.Play(SEManager.SEType.FOOTSTEPS);
    }

}
