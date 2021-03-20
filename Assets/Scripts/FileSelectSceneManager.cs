using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileSelectSceneManager : MonoBehaviour
{
    [SerializeField] GameObject saveDataUIPrefab;
    [SerializeField] Transform dataFileContainer;
    [SerializeField] int saveDataCount = 0;
    List<SaveDataUI> m_saveDatas = new List<SaveDataUI>();
    private void Awake()
    {
        for (int i = 0; i < saveDataCount; i++)
        {
            var but = Instantiate(saveDataUIPrefab, dataFileContainer).GetComponent<Button>();
            but.onClick.AddListener(() =>
            {
                OnClickSaveData(i);
            });
            m_saveDatas.Add(but.GetComponent<SaveDataUI>());
        }
    }
    private void Start()
    {
        for (int i = 0; i < saveDataCount; i++)
        {
            var item = SaveDataReader.Instance.GetFileHeader(i);
            if (item==null)
            {
                m_saveDatas[i].Initialize($"DataFile ???", $"0.0");
            }
            else
            {
                m_saveDatas[i].Initialize($"DataFile {i}", $"{item.loopCount}.{item.chapterCount}");
            }
        }
        
    }

    public void OnClickSaveData(int index)
    {

    }
}
