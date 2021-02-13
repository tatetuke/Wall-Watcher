//https://qiita.com/toRisouP/items/4445b6b9bf00e49eb147
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UniTaskTest : MonoBehaviour
{
    [SerializeField] private AssetLabelReference _labelReference;
    AsyncOperationHandle<IList<ConversationData>> m_handle;
    public Dictionary<string, ConversationData> m_data = new Dictionary<string, ConversationData>();

    // Start is called before the first frame update
    private async void Start()
    {
        await LoadTest();

        ////値が返されるまで待つ
        //int a = await UniTaskTestInt(10);
        ////返される値を無視して次の行へ進む 処理を投げっぱなし
        //UniTaskTestInt(100).Forget();
        ////値が返されるまで待つ
        //int b = await UniTaskTestInt(9);
    }

    private async UniTask<int> UniTaskTestInt(long x)
    {
        //Debug.Log("<color=#0a0> 処理開始" + x + "</color>");
        await UniTask.Delay(2000);
        return await UniTask.Run(() =>
        {
            for (int i = 0; i < 10000000; ++i)
            {
                x = x * x % 100000007;
            }
            //  Debug.Log($"<color=#9a0>処理終了 {x} </color>");
            return (int)x;
        });
    }

    private async UniTask LoadTest()
    {
        Debug.Log("start loading");

        m_handle = Addressables.LoadAssetsAsync<ConversationData>(_labelReference, null);
        await m_handle.Task;
        foreach (var res in m_handle.Result)
        {
            m_data.Add(res.name, res);
            Debug.Log($"Load Conversation: '{res.name}'");
        }
        Addressables.Release(m_handle);
        Debug.Log("end loading");
    }

    
}
