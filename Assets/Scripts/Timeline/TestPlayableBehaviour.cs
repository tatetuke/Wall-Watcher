using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// 実際にアニメーションを実行する際の処理を書かれている
/// </summary>

// A behaviour that is attached to a playable
public class TestPlayableBehaviour : PlayableBehaviour
{
    public GameObject Player;

    //private GameObject mPlayer;
    //public GameObject Player { set { mPlayer = value; } }


    // タイムライン開始時に呼び出される
    public override void OnGraphStart(Playable playable)
    {
        
    }

    // タイムライン停止時に呼び出される
    public override void OnGraphStop(Playable playable)
    {
        
    }

    // タイムラインでこのスクリプトが実行されたときに呼び出される
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        //Vector3 vel = new Vector3(-5, 0, 0);
        //Player.transform.position = Player.transform.position + vel;
        //Debug.Log(Player.transform.position.x);
    }

    // タイムラインでこのスクリプトが一時停止した時に呼び出される
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        
    }

    // アニメーションの各フレームごとに呼び出される
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        Vector3 vel = new Vector3(-0.01f, 0, 0);
        Player.transform.position = Player.transform.position + vel;
    }
}
