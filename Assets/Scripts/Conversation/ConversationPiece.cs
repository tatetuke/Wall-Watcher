using System.Collections.Generic;
using RPGM.Gameplay;
using UnityEngine;

namespace RPGM.Gameplay
{

    public enum ConversationType
    {
        normal,
        events
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
        //public Quest quest;
        //選択肢
        public List<ConversationOption> options;

        public string eventName;
    }

}