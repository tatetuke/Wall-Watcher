using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationModule : MonoBehaviour
{
    [SerializeField]
    private State m_State = default;
    [SerializeField]
    private Animator m_Animator = default;


    public enum State
    {
        IDLE,//立ち状態
        WALKING,//歩き
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateState();
    }

    public virtual void ChangeState(State state)
    {
        m_State = state;
        if (m_State == State.IDLE)
        {
            m_Animator.SetBool("IsWalking", false);
        }
        else if (m_State == State.WALKING)
        {
            m_Animator.SetBool("IsWalking", true);
        }
        else
        {
            Debug.LogWarning("対応できていない状態です");
        }
    }

    protected virtual void UpdateState()
    {
        if (m_State == State.IDLE)
        {

        }
        else if (m_State == State.WALKING)
        {
         
        }
        else
        {
            Debug.LogWarning("対応できていない状態です");
        }
    }
}
