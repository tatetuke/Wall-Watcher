using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HAKAIGuideManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GuideManager.m_State);
        // ガイドのとき or Hキー(Help)を押したら説明開始
        if (GuideManager.m_State==GuideManager.State.Mix || Input.GetKeyDown(KeyCode.H))
        {
            Flowchart.BroadcastFungusMessage("HAKAIGuideStart");
        }
    }

    public void ChangeState()
    {
        GuideManager.m_State = GuideManager.State.AfterHAKAI;
    }
}