using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class Define
{
    /// <summary>
    /// NPCとPlayerの距離としてありうる最大の距離。
    /// </summary>
    public const float InfDis = 10000;
}

public class SearchNearNPC : MonoBehaviour
{

    [SerializeField] GameObject PlayerObj;//プレイヤー
   
    ///テスト用
    //GameObject tmp;
    //private void Update()
    //{
    //    tmp = NearNPC();
    //}


    
    /// <summary>
    /// 会話可能なNPCのうち、最短距離のNPCを返す関数
    /// 会話可能なNPCがいない場合はnullを返す
    /// </summary>
    /// <returns></returns>
    GameObject NearNPC()
    {
        float minDis = Define.InfDis;   //NPCとの最短距離
        float tmpDis;                   //距離用一時変数
        GameObject retNearNPC = null; //返り値

        //プレイヤーと話せるNPCを全探索
        foreach (GameObject NPC in GameObject.FindGameObjectsWithTag("CanConversationNPC"))
        {
    
            //自身と取得したオブジェクトの距離を取得
            tmpDis = Vector2.Distance(NPC.transform.position, PlayerObj.transform.position);

            //NPCとプレイヤーの距離が現在の最短距離より近ければ更新
            if (minDis > tmpDis)
            {
                //NPCの最短距離を更新;
                minDis = tmpDis;
                //一番近いNPCを更新
                retNearNPC = NPC;
            }

        }
        if (retNearNPC != null) Debug.Log($"NearNPC is {retNearNPC.name}");
        //最も近いNPCを返す
        return retNearNPC;
    }
            
}
