using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// プレイヤーのデータを読み書きするクラス
/// また、ゲーム開始時にセーブデータからプレイヤーを生成する
/// </summary>
public class PlayerDataLoader : MonoBehaviour,ISaveable,ILoadable
{

    [SerializeField] GameObject playerPrefab;
     GameObject currentPlayer;//現在のプレイヤーを記録する
    [SerializeField] string filedirectory;//セーブデータの存在するディレクトリ
    [SerializeField] string filename;//セーブデータの名前（拡張子もつけて(.csv推奨)）

    // Start is called before the first frame update
    void Awake()
    {
        SaveLoadManager.Instance.SetLoadable(this);
        SaveLoadManager.Instance.SetSaveable(this);
    }

    void ILoadable.Load()
    {
        currentPlayer = Instantiate(playerPrefab);
        var obj=CSVReader.Read(filedirectory, filename);
        foreach(var s in obj)//csvの各行について実行
        {
            switch (s[0])
            {
                case "player.position"://1列目がplauer.positionなら2～4列目までから座標を読み込む
                    Vector3 playerPos=Vector3.zero;
                    playerPos.x = float.Parse(s[1]);
                    playerPos.y = float.Parse(s[2]);
                    playerPos.z = float.Parse(s[3]);
                    currentPlayer.transform.position = playerPos;
                    break;
            }
        }
    }

    void ISaveable.Save()
    {
        List<List<string>> data = new List<List<string>>();
        data.Add(new List<string>{ 
        "player.position",
        currentPlayer.transform.position.x.ToString(),
        currentPlayer.transform.position.y.ToString(),
        currentPlayer.transform.position.z.ToString(),
        });
        CSVReader.Write(filedirectory, filename,data);
    }
}
