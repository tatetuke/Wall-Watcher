using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 画像を虹色にする
/// </summary>
public class HakaiRainbow : MonoBehaviour
{
    public Image image;
    public int hue=0;
    public float saturation=1;
    public float value=1;
    private int span = 10;
    private int time = 0;
    // Update is called once per frame
   
    
    [Header("変更の滑らかさ")]
    public float Smooth = 0.001f;

    [Header("色彩")]
    [Range(0, 1)] public float HSV_Hue = 1.0f;// 0 ~ 1

    [Header("彩度")]
    [Range(0, 1)] public float HSV_Saturation = 1.0f;// 0 ~ 1

    [Header("明度")]
    [Range(0, 1)] public float HSV_Brightness = 1.0f;// 0 ~ 1

    [Header("色彩 MAX")]
    [Range(0, 1)] public float HSV_Hue_max = 1.0f;// 0 ~ 1

    [Header("色彩 MIN")]
    [Range(0, 1)] public float HSV_Hue_min = 0.0f;// 0 ~ 1

    // Start is called before the first frame update
    private void Update()
    {
        //if (time == 0)
        //{
            HSV_Hue += Smooth;

            if (HSV_Hue >= HSV_Hue_max)
            {
                HSV_Hue = HSV_Hue_min;
            }

            image.color = Color.HSVToRGB(HSV_Hue, HSV_Saturation, HSV_Brightness);

        //}
        //time++;
        //time %= span;
    }
}
