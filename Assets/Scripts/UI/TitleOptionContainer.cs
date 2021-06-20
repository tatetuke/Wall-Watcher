using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleOptionContainer : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] Image image;
    public bool enableFlag = true;//有効かどうか
    private void Start()
    {
        SetInactive();
    }
    /// <summary>
    /// タイトル画面の選択肢において、テキストとスプライトの色を薄くする関数
    /// </summary>
    public void SetInactive()
    {
        if (!enableFlag)
        {
            text.color = new Color32(0, 0, 0, 50);
        }
        else
        {
            text.color = new Color32(0, 0, 0, 100);
        }
        //見えなくする
        image.gameObject.SetActive(false);
    }
    /// <summary>
    /// タイトル画面の選択肢において、テキストとスプライトの色を濃くする関数
    /// </summary>
    public void SetActive()
    {
        image.gameObject.SetActive(true);
        if (!enableFlag)
        {
            text.color = new Color32(0, 0, 0, 127);
            image.color = new Color32(255, 255, 255, 127);
        }
        else
        {
            //黒
            text.color = new Color32(0, 0, 0, 255);
            //元画像のまま
            image.color = new Color32(255, 255, 255, 255);
        }
    }
}
