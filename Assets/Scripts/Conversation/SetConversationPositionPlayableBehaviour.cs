using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class SetConversationPositionPlayableBehaviour : PlayableBehaviour
{
    public GameObject Player;
    public GameObject TargetNPC;
    public Vector3 startPosition;
    public Vector3 endPosition;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        startPosition = Player.transform.position;
        TargetNPC = ConversationDataManager.Instance.GetTargetNPC();

        // MEMO : 元からNPCより右にいたらNPCの右側
        //                     左             左

        // 歩きながら話しかけたときの例外処理


        endPosition.x = TargetNPC.transform.position.x + 1;
        endPosition.y = startPosition.y;
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        var t = (float)playable.GetTime() / (float)playable.GetDuration();
        float leapt = (t * t) * (3f - (2f * t));
        Player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, Mathf.Clamp01(leapt));
    }
}