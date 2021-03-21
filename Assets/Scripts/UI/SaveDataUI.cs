using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SaveDataUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI chapterNum;

    public void Initialize(string dataFileName,string chapterNum_)
    {
        text.text = dataFileName;
        chapterNum.text = chapterNum_;
    }
}
