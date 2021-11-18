using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGuideManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ガイドのとき or Hキー(Help)を押したら説明開始
        if (GuideManager.m_State == GuideManager.State.AfterHAKAI || Input.GetKeyDown(KeyCode.H))
        {
            Flowchart.BroadcastFungusMessage("PaintGuideStart");
        }
    }

    public void ChangeState()
    {
        GuideManager.m_State = GuideManager.State.AfterPaint;
    }
}
