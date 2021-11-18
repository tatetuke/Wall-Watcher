using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideManager : MonoBehaviour
{
    public static State m_State = State.NotGuide;

    public enum State
    {
        NotGuide,
        Mix,
        AfterHAKAI,
        AfterPaint
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (m_State==State.NotGuide && Input.GetKeyDown(KeyCode.G))
        {
            ChangeState(State.Mix);
            Flowchart.BroadcastFungusMessage("GuideStart");
        }

        if (m_State == State.AfterHAKAI)
        {
            Flowchart.BroadcastFungusMessage("MoveToPaintStart");
        }

        if (m_State == State.AfterPaint)
        {
            Flowchart.BroadcastFungusMessage("ReceiveRewards");
        }
    }

    public void ChangeState(State state)
    {
        m_State = state;
    }
}
