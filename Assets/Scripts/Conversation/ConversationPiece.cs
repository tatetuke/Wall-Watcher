using System.Collections.Generic;
using RPGM.Gameplay;
using UnityEngine;
using UnityEngine.Playables;

namespace RPGM.Gameplay
{

    [System.Serializable]
    public class Conversations
    {
        public string id="";
        public string targetID="";

        [Multiline]
        public string text="";
        //public TalkData.TalkType talkType;
        //public TalkData.FaceType talkFace;
        public PlayableDirector playableDirector;
        //テクストを表示するときに出る効果音
        public AudioClip audio;
        public QuestDataSO quest;
        //開始条件
        public List<QuestConditions> conditions=new List<QuestConditions>();
        //選択肢
        public List<ConversationOption> options=new List<ConversationOption>();
        //イベント発行用（使ってない）
        public string eventName="";
        //別の会話へのリンク（使ってない）
        public Conversations subConversation;
    }

}