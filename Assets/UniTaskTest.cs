//https://qiita.com/toRisouP/items/4445b6b9bf00e49eb147
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class UniTaskTest : MonoBehaviour
{
    // Start is called before the first frame update
    private async void Start()
    {
        //値が返されるまで待つ
        int a = await UniTaskTestInt(10);
        //返される値を無視して次の行へ進む 処理を投げっぱなし
        UniTaskTestInt(100).Forget();
        //値が返されるまで待つ
        int b = await UniTaskTestInt(9);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private async UniTask<int> UniTaskTestInt(long x)
    {
        Debug.Log("<color=#0a0> 処理開始" + x + "</color>");
        await UniTask.Delay(2000);
        return await UniTask.Run(() =>
        {
            for (int i = 0; i < 10000000; ++i)
            {
                x = x * x % 100000007;
            }
            Debug.Log($"<color=#9a0>処理終了 {x} </color>");
            return (int)x;
        });
    }
}
