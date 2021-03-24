using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveUIView : UIView
{
    [SerializeField] Transform contentParent;
    [SerializeField] GameObject saveFilePrefab;
    [SerializeField] Button successPopUp; 
    List<SaveUI> m_saveDatas = new List<SaveUI>();
    //[SerializeField] int saveDataCount = 0;
    // Start is called before the first frame update
    void Start()
    {
            successPopUp.gameObject.SetActive(false);
        for (int i = 0; i < SaveDataReader.Instance.GetFileCount(); i++)
        {
            var scr = Instantiate(saveFilePrefab, contentParent).GetComponent<SaveUI>();
            m_saveDatas.Add(scr);
            var item = SaveDataReader.Instance.GetFileHeader(i);
            if (item == null)
            {
                m_saveDatas[i].Initialize($"DataFile ???", $"0.0", i, OnClickSaveData);
            }
            else
            {
                m_saveDatas[i].Initialize($"DataFile {i}", $"{item.loopCount}.{item.chapterCount}", i, OnClickSaveData);
            }
        }
        successPopUp.onClick.AddListener(() =>
        {
            successPopUp.gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// セーブデータのスロットをクリックしたとき、セーブデータ内にあるシーン名をロードする
    /// </summary>
    /// <param name="index"></param>
    public void OnClickSaveData(int index)
    {
        SaveDataWriter.Instance.Save(index);
        successPopUp.gameObject.SetActive(true);
    }
}
