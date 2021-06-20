using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Save.Test
{
    [System.Serializable]
    public class ExampleSaveData : SaveDataBaseClass
    {
        public string text;
        public Color color;
        public Vector3 position;
    }

    //セーブ・ロードを行うMonoBehaviourの見本
    //Save(), Load()、使われるkeyをusedKeyListに追加することが必須
    public class SaveLoadableMonoBehaviourTest : SaveLoadableMonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI text;
        [SerializeField] private string saveKey;
        [SerializeField] private ExampleSaveData exampleSaveData = new ExampleSaveData();

        protected override void Save()
        {
            DataBank.Instance.Store(saveKey, exampleSaveData);
            DataBank.Instance.Save(saveKey);
        }

        protected override void Load()
        {
            DataBank.Instance.Load<ExampleSaveData>(saveKey);
            exampleSaveData = DataBank.Instance.Get<ExampleSaveData>(saveKey);
        }

        protected override List<string> GetKeyList()
        {
            return new List<string>() { saveKey };
        }

        protected override void Awake()
        {
            //base.Awake()を忘れない SaveLoadManagerでの重複チェック・登録が行われる
            base.Awake();

        }
        
        public async void SaveTest()
        {
            Debug.Log("Save");
            await SaveLoadManager.Instance.SaveAllAsync();
        }

        public async void LoadTest()
        {
            Debug.Log("Load");
            await SaveLoadManager.Instance.LoadAllAsync();
            text.text = exampleSaveData.text;
            text.color = exampleSaveData.color;
            text.gameObject.transform.localPosition = exampleSaveData.position;
        }

        protected override UniTask SaveAsync()
        {
            throw new NotImplementedException();
        }

        protected override UniTask LoadAsync()
        {
            throw new NotImplementedException();
        }
    }
}