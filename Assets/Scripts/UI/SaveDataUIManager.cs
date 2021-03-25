using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveDataUIManager : UIView
{
    [SerializeField] GameObject saveDataUIPrefab;
    [SerializeField] Transform dataFileContainer;
    [SerializeField] string backSceneName = "TItleScene";
    List<SaveDataUI> m_saveDatas = new List<SaveDataUI>();
    //[SerializeField] int saveDataCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < SaveDataReader.Instance.GetFileCount(); i++)
        {
            var scr = Instantiate(saveDataUIPrefab, dataFileContainer).GetComponent<SaveDataUI>();
            m_saveDatas.Add(scr);
            var item = SaveDataReader.Instance.GetFileHeader(i);
            if (item == null)
            {
                m_saveDatas[i].Initialize($"DataFile ???", $"0.0",i, OnClickSaveData);
            }
            else
            {
                m_saveDatas[i].Initialize($"DataFile {i}", $"{item.loopCount}.{item.chapterCount}",i, OnClickSaveData);
            }
        }
        backButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(backSceneName);
        });
    }

    /// <summary>
    /// セーブデータのスロットをクリックしたとき、セーブデータ内にあるシーン名をロードする
    /// </summary>
    /// <param name="index"></param>
    public void OnClickSaveData(int index)
    {
      var data=  SaveDataReader.Instance.GetFileData(index);
        SceneManager.LoadScene(data.roomName);
    }
}
