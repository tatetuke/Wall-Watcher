using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleShopItemLoader
{
    private string saveKey = "sampleShopItem";
    // Start is called before the first frame update
    private SampleShopLineUp sampleLineUp = new SampleShopLineUp();
    private void Start()
    {
        Load();
    }
    protected void Save()
    {
        DataBank.Instance.Store(saveKey, sampleLineUp);
        DataBank.Instance.Save(saveKey);
    }
    protected void Load()
    {
        DataBank.Instance.Load<SampleShopLineUp>(saveKey);
        sampleLineUp = DataBank.Instance.Get<SampleShopLineUp>(saveKey);
    }
}
