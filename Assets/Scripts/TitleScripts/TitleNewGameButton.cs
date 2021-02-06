using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TitleNewGameButton : MonoBehaviour
{
   /// <summary>
   /// ニューゲームのボタンが押された時の処理
   /// </summary>
    public void OnClick()
    {
        Debug.Log("PlayNewGame");
        FadeManager.Instance.LoadLevel("SampleMainScene", 1.5f);
    }

}
