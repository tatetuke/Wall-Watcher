using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class SetConversationPositionPlayableBehaviour : PlayableBehaviour
{
    public GameObject Player;
    public GameObject PlayerSprite;
    public float Distance;
    public GameObject TargetNPC;
    public Vector3 startPosition;
    public Vector3 endPosition;

    // タイムライン開始時に呼び出される
    public override void OnGraphStart(Playable playable)
    {
        PlayerSprite = Player.transform.Find("PlayerSprite").gameObject;
        TargetNPC = ConversationDataManager.Instance.GetTargetNPC();

        startPosition = Player.transform.position;

        Quaternion quaternion = PlayerSprite.transform.rotation;
        float PlayerSprite_rotation_y = quaternion.eulerAngles.y;

        // プレイヤーが移動する方向に向くようにする
        if (Player.transform.position.x + Distance < TargetNPC.transform.position.x)
            PlayerSprite.transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (Player.transform.position.x < TargetNPC.transform.position.x)
            PlayerSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (Player.transform.position.x - Distance > TargetNPC.transform.position.x)
            PlayerSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            PlayerSprite.transform.rotation = Quaternion.Euler(0, 180, 0);

        // NPCより右にいたら右の定位置に、左にいたら左の定位置に
        if (startPosition.x < TargetNPC.transform.position.x)
            endPosition.x = TargetNPC.transform.position.x - Distance;
        else
            endPosition.x = TargetNPC.transform.position.x + Distance;
        endPosition.y = startPosition.y;  // y座標は最初と同じ
        endPosition.z = 0;
    }

    // タイムライン停止時に呼び出される
    public override void OnGraphStop(Playable playable)
    {
        // プレイヤーが対象のNPCの方向に向くようにする
        if (Player.transform.position.x < TargetNPC.transform.position.x)
            PlayerSprite.transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            PlayerSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // タイムラインでこのスクリプトが実行されたときに呼び出される
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {

    }

    // タイムラインでこのスクリプトが一時停止した時に呼び出される
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {

    }

    // アニメーションの各フレームごとに呼び出される
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        var t = (float)playable.GetTime() / (float)playable.GetDuration();
        float leapt = (t * t) * (3f - (2f * t));
        Player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, Mathf.Clamp01(leapt));
    }
}