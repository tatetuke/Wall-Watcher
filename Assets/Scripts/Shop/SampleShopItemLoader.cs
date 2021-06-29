using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleShopItemLoader : SaveLoadableMonoBehaviour
{
    private string saveKey = "sampleShopItem";
    // Start is called before the first frame update
    private SampleShopLineUp sampleLineUp = new SampleShopLineUp();
    private void Start()
    {
        Load();
    }
    protected override void Save()
    {
        DataBank.Instance.Store(saveKey, sampleLineUp);
        DataBank.Instance.Save(saveKey);

    }
    protected override void Load()
    {
        DataBank.Instance.Load<SampleShopLineUp>(saveKey);
        sampleLineUp = DataBank.Instance.Get<SampleShopLineUp>(saveKey);
    }

    protected override UniTask SaveAsync()
    {
        throw new System.NotImplementedException();
    }

    protected override UniTask LoadAsync()
    {
        throw new System.NotImplementedException();
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
}
