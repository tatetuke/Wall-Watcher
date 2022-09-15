using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class StateMachineCallback : StateMachineBehaviour
{

    public UnityEvent onStateEnter=new UnityEvent();
    public UnityEvent onStateExit = new UnityEvent();

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onStateEnter.Invoke();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onStateExit.Invoke();
    }

}