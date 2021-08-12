﻿using UnityEngine;
using System.Collections;

/// パーティクル
public class Particle : Token
{
    /// プレハブ
    static GameObject _prefab = null;
    /// パーティクルの生成
    public static Particle Add(float x, float y)
    {
        // プレハブを取得
        _prefab = GetPrefab(_prefab, "Particle");
        // プレハブからインスタンスを生成
        return CreateInstance2<Particle>(_prefab, x, y);
    }

    /// 開始。コルーチンで処理を行う
    IEnumerator Start()
    {
        // 移動方向と速さをランダムに決める
        float dir = Random.Range(0, 359);
        float spd = Random.Range(10.0f, 20.0f);
        SetVelocity(dir, spd);

        // 見えなくなるまで小さくする
        while (ScaleX > 0.01f)
        {
            // 0.01秒ゲームループに制御を返す
            yield return new WaitForSeconds(0.01f);
            // だんだん小さくする
            MulScale(0.9f);
            // だんだん減速する
            MulVelocity(0.9f);
        }

        // 消滅
        DestroyObj();
    }
}
