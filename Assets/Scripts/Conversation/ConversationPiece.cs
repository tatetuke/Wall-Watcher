using System.Collections.Generic;
using RPGM.Gameplay;
using UnityEngine;

namespace RPGM.Gameplay
{

    public enum ConversationType
    {
        normal=0,
        events=1,
        subConversation=2,
    }

    [System.Serializable]
    public class Conversations
    {
       public ConversationType type;

        public string id;
        public string targetID;

        [Multiline]
        public string text;
        // public TalkerData talker;
        //public TalkData.TalkType talkType;
        //public TalkData.FaceType talkFace;
        //テクストを表示するときに出る効果音
        public AudioClip audio;
        public QuestDataSO quest;
        //選択肢
        public List<ConversationOption> options;
        //イベント発行用（使ってない）
        public string eventName;
        //別の会話へのリンク（使ってない）
        public Conversations subConversation;
    }

}