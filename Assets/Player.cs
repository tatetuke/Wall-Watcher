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
    private State m_State = default;

    private CharacterController m_CharacterController = default;

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
        m_CharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal") * m_WalkSpeed;
        Vector3 vel = new Vector3(moveX, 0, 0);
        m_CharacterController.SimpleMove(vel);
        
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
