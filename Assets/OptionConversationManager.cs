using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionConversationManager : MonoBehaviour
{
    public int res = 0;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetBorder()
    {
        Debug.Log("呼び出された");
        // セレクトに関する更新
        if (Input.GetKeyDown("left"))
        {
            Debug.Log("left");
            //m_dialogController.Hide(Borders[m_selectManager.GetSelectNum()]);
            //m_selectManager.UpdateLeft();   // 左押したときに関する更新
            //m_dialogController.Display(Borders[m_selectManager.GetSelectNum()]);
        }
        if (Input.GetKeyDown("right"))
        {
            Debug.Log("right");
            //m_dialogController.Hide(Borders[m_selectManager.GetSelectNum()]);
            //m_selectManager.UpdateRight();  // 右押したときに関する更新
            //m_dialogController.Display(Borders[m_selectManager.GetSelectNum()]);
        }
    }
}
