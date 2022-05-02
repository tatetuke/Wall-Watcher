using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

//新しくセーブしたい項目を増やしたくなったときの見本

namespace Save.Test {

    //　基本的にはSaveDataBaseを継承していれば大丈夫
    //　継承する理由はToString()を上書きする必要があるため
    [System.Serializable]
    public class AudioSettingData : SaveDataBaseClass
    {
        public List<float> volumes = new List<float>(3);

        public string ToData()
        {
                return $"{base.ToString()} {JsonUtility.ToJson(this)}";
        }
        //いつもInspectorに表示されるような単純な型は保存できる
        //GameObject型やTransform型なども一応できる
    }

    [System.Serializable] //別にSerializableにしなくてもよい
    public class UISetting : SaveDataBaseClass
    {
        public float size;
        public Color color;
        //クラスや構造体の入れ子も可能
        public MyClass myClass;
        public Transform transform;
        public GameObject gameObject;
        public string ToData()
        {
            return $"{base.ToString()} {JsonUtility.ToJson(this)}";
        }
    }

    public class MyClass
    {
        int abc;
    }

    /// <summary>
    /// DataBankのテスト用
    /// </summary>
    public class SaveTest : MonoBehaviour
    {
        public SaveData data;
        public AudioSettingData audioSettingData;
        public UISetting uISetting;

        public void Start()
        {
            audioSettingData = new AudioSettingData() { volumes = new List<float> { 1, 0.5f, 0.75f } };
            uISetting = new UISetting() { size = 10, color = new Color(1, 1, 0) ,
                transform = this.transform, gameObject = this.gameObject};

            DataBank bank = DataBank.Instance;
            //一時データに保管
            bank.Store("audio", audioSettingData);
            bank.Store("ui_setting", uISetting);
            //ファイルへと保存
            bank.SaveAll();
            //一時データを消去
            bank.Clear();
            //ファイルから読み込む
            bank.Load<AudioSettingData>("audio");
            bank.Load<UISetting>("ui_setting");
            Debug.Log("Loaded");
            //取得
            Debug.Log(bank.Get<AudioSettingData>("audio"));
            Debug.Log(bank.Get<UISetting>("ui_setting"));

            Test();
        }


        public void Test()
        {
            DataBank bank = DataBank.Instance;
            Debug.Log("DataBank.Open()");
            Debug.Log($"save path of bank is { bank.SavePath }");

            //テスト用のセーブデータを定義
            SaveData saveData = new SaveData()
            {
                loopCount=3,
                chapterCount=1,
                playerPosition = new Vector3(10, 0, 0),
                roomName = "room1",
                money = 999,
                inventry = new List<Kyoichi.ItemStack>(),
                quests = new List<QuestSaveData> {
                    new QuestSaveData { questName="quest1",cuestChapter=1,state=QuestChecker.QuestState.not_yet}
                },
            };
            Debug.Log(saveData);

            //データをキーに紐づけて一時的に保管　ファイルへ保存はされていないので注意
            bank.Store("player", saveData);
            Debug.Log("bank.Store()");

            //保管しているデータをすべてファイルへと書き込んで保存
            bank.SaveAll();
            Debug.Log("bank.SaveAll()");

            SaveData storedData = new SaveData();
            Debug.Log("default savedata");
            Debug.Log(storedData);

            //保管された一時データから取得　セーブされたファイルからではないので注意
            storedData = bank.Get<SaveData>("player");
            Debug.Log("bank.Get<SaveData>(\"player\")");
            Debug.Log(storedData);

            //保管された一時データを消去
            bank.Clear();
            Debug.Log("bank.Clear()");

            //保管された一時データから読み込む　Clearしたので null のはず
            storedData = bank.Get<SaveData>("player");
            Debug.Log(storedData);

            //指定したキーでデータをファイルから読み込む
            bank.Load<SaveData>("player");
            Debug.Log("bank.Load()");

            //保管された一時データから読み込む
            storedData = bank.Get<SaveData>("player");
            Debug.Log(storedData);

            data = storedData;
        }
    }
}