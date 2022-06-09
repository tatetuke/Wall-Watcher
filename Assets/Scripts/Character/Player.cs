using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using map;

/// <summary>
/// プレイヤーの移動・見た目周りを管理するクラス
/// </summary>
public class Player : MonoBehaviour
{
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

    [SerializeField]
    private float m_Inertia; // 慣性 0なら止まる
    [SerializeField]
    private float m_InertiaMultiplier;

    public float past_walkForce { get; private set; } = 0.0f;
    //public float autoMoveTime { get; private set; } = 0.0f;
    //public IEnumerator _v_autoMove { get; private set; } = null;

    public enum State
    {
        FREEZE,//移動不可能
        IDLE,//立ち状態
        WALKING,//歩き
        MINIGAME,//ミニゲーム
        AUTOR,//自動歩行（右方向）
        AUTOL,//自動歩行（左方向）
    }

    public bool IsWalking
    {
        get { return m_Inertia != 0; }
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(State.IDLE);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.T))
            ChangeState(State.FREEZE);
        if (Input.GetKeyDown(KeyCode.Y))
            ChangeState(State.IDLE);
        UpdateState();
    }

    /// <summary>
    /// 入力の様子によって動きを決める
    /// ゆっくりと止まる部分も担当
    /// </summary>
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
        //float moveX = Input.GetAxisRaw("Horizontal") * m_Inertia * m_WalkForce;
        float moveX = m_Inertia * m_WalkForce;
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

    /// <summary>その場で即座にピタッと止めたい場合</summary>
    public void Stop()
    {
        m_Inertia = 0;
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
        else if(m_State == State.AUTOR || m_State == State.AUTOL)
        {
            m_AnimationModule.ChangeState(AnimationModule.State.WALKING);
        }
        else
        {
            Debug.LogWarning("対応できていない状態です");
        }
    }

    

    private void UpdateState()
    {
        Move();
        if (m_State == State.FREEZE)
        {
            UpdateInertia(0);//慣性を減衰させる

            if (IsWalking)
                m_AnimationModule.ChangeState(AnimationModule.State.WALKING);
            else
                m_AnimationModule.ChangeState(AnimationModule.State.IDLE);
        }
        else if (m_State == State.IDLE)
        {
            UpdateInertia(Input.GetAxisRaw("Horizontal"));

            if (IsWalking)
                ChangeState(State.WALKING);
        }
        else if (m_State == State.WALKING)
        {
            UpdateInertia(Input.GetAxisRaw("Horizontal"));

            if (!IsWalking)
                ChangeState(State.IDLE);
        }
        else if (m_State == State.MINIGAME)
        {

        }
        else if(m_State == State.AUTOR)
        {
            UpdateInertia(1);

            if (!IsWalking)
                ChangeState(State.IDLE);
        }
        else if (m_State == State.AUTOL)
        {
            UpdateInertia(-1);

            if (!IsWalking)
                ChangeState(State.IDLE);
        }
        else
        {
            Debug.LogWarning("対応できていない状態です");
        }
    }

    public void PlayFootStepSE()
    {
        //SEManager.Instance.Play("footsteps");
    }

    /// <summary>
    /// -1~1の値を受け取り慣性を更新する
    /// </summary>
    /// <param name="inputValue">-1~1の値</param>
    private void UpdateInertia(float inputValue)
    {
        float delta = (inputValue >= 0 ? Time.deltaTime : -Time.deltaTime) * m_InertiaMultiplier;
        
        //入力がある場合
        if (inputValue != 0)
        {
            //現在の方向と入力の方向が一緒なら
            if (m_Inertia * delta >= 0)
            {
                m_Inertia = Mathf.Clamp(m_Inertia + delta, -1, 1);
            }
            else
            {
                m_Inertia = Mathf.Clamp(delta, -1, 1); ;
            }
            return;
        }

        //入力が無い場合
        //減衰させる
        if (m_Inertia >= 0)
            m_Inertia = Mathf.Clamp(m_Inertia - Time.deltaTime * m_InertiaMultiplier, 0, 1);
        else
            m_Inertia = Mathf.Clamp(m_Inertia + Time.deltaTime * m_InertiaMultiplier, -1, 0);

    }

    /// <summary>
    /// 自動移動関数（後に改良予定）
    /// stopAutoMove() を呼ばない限り止まらない
    /// </summary>
    /// <param name="auto_speed"> 0以上の移動時にかかる力の強さ</param>
    /// <param name="auto_direction">移動方向 Left か Right</param>
    public void autoMove(float auto_speed, Direction2D auto_direction)
    {
        past_walkForce = m_WalkForce;
        m_WalkForce = auto_speed;
        AllMapSet.autoWalkingDirection = auto_direction;
        v_autoMove();
    }

    private void v_autoMove()
    {
        if (AllMapSet.autoWalkingDirection == Direction2D.Right)
        {
            ChangeState(State.AUTOR);
        }
        else if (AllMapSet.autoWalkingDirection == Direction2D.Left)
        {
            ChangeState(State.AUTOL);
        }
    }

    IEnumerator v_stopAutoMove()
    {
        ChangeState(State.IDLE);
        yield return new WaitForSeconds(1f);
        m_WalkForce = past_walkForce;
    }

    public void stopAutoMove()
    {
        StartCoroutine(v_stopAutoMove());
    }
}
