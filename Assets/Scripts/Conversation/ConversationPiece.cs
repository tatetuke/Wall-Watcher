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
        //�e�N�X�g��\������Ƃ��ɏo����ʉ�
        public AudioClip audio;
        public QuestDataSO quest;
        //�J�n����
        public List<QuestConditions> conditions=new List<QuestConditions>();
        //�I����
        public List<ConversationOption> options=new List<ConversationOption>();
        //�C�x���g���s�p�i�g���ĂȂ��j
        public string eventName="";
        //�ʂ̉�b�ւ̃����N�i�g���ĂȂ��j
        public Conversations subConversation;
    }

}