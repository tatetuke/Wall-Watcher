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
                m_saveDatas[i].Initialize($"DataFile ???", $"0.0", i, OnClickSaveData);
            }
            else
            {
                m_saveDatas[i].Initialize($"DataFile {i}", $"{item.loopCount}.{item.chapterCount}", i, OnClickSaveData);
            }
        }
        backButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(backSceneName);
        });
    }
    SaveData m_loadedData;
    /// <summary>
    /// セーブデータのスロットをクリックしたとき、セーブデータ内にあるシーン名をロードする
    /// </summary>
    /// <param name="index"></param>
    public void OnClickSaveData(int index)
    {
        m_loadedData = SaveDataReader.Instance.GetFileData(index);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(m_loadedData.roomName);
    }

    /// <summary>
    /// ゲームのシーンがロードされたときに実行される関数
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("Load scene from saveData");
        var playerScr = FindObjectOfType<Player>();
        if (playerScr == null) Debug.LogError("player not found");
        else
        {
            playerScr.transform.position = m_loadedData.playerPosition;
        }
        var moneyScr = FindObjectOfType<MoneyScript>();
        if (moneyScr == null) Debug.LogError("money not found");
        else
        {
            moneyScr.Money = m_loadedData.money;
        }
        var inventryScr = FindObjectOfType<Kyoichi.Inventry>();
        if (inventryScr == null) Debug.LogError("inventry not found");
        else
        {
            foreach(var i in m_loadedData.inventry)
            {
                inventryScr.AddItem(i);
            }
        }
        var questScr = FindObjectOfType<QuestHolder>();
        if (questScr == null) Debug.LogError("quest not found");
        else
        {
            foreach (var i in m_loadedData.quests)
            {
               var data= QuestsManager.Instance.GetQuest(i.questName);
                if (data == null)
                {
                    Debug.LogWarning("data is null");
                    continue;
                }
                questScr.Initialize(data,i.state,i.cuestChapter);
            }
        }

    }
}
