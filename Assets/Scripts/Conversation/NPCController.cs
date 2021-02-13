using RPGM.Core;
using RPGM.Gameplay;
using UnityEngine;
using UnityEngine.UI;
namespace RPGM.Gameplay
{
    /// <summary>
    /// Main class for implementing NPC game objects.
    /// </summary>
    public class NPCController : MonoBehaviour
    {
        [SerializeField] Text TextBox;
        public ConversationData[] ConversationsList;
        Conversations CurrentConversation;
        string id;
        private void Start()
        {
            if (ConversationsList.Length == 0) return ;
            id=ConversationsList[0].GetFirst();
            CurrentConversation = ConversationsList[0].Get(id);
        }
        //Quest activeQuest = null;

        //Quest[] quests;

        //GameModel model = Schedule.GetModel<GameModel>();

        //void OnEnable()
        //{
        //    quests = gameObject.GetComponentsInChildren<Quest>();
        //}

        void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("NPCと接近!");
                Texting();

            }
            
        }


        /// <summary>
        /// 文章を表示します。
        /// スペースキーが押されたときに文章を送ります。
        /// </summary>
        private void Texting()
        {
            if (ConversationsList.Length == 0) return;
            TextBox.text = CurrentConversation.text;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ConversationsList[0].Delete(id);
                id = CurrentConversation.targetID;
                CurrentConversation = ConversationsList[0].Get(id);

            }
        }

        // private string Texting(Conversations CurrentConversation)
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        return CurrentConversation.targetID;

        //    }
        //    return CurrentConversation.id;
        //}
        public void OnCollisionEnter2D(Collision2D collision)
        {
            //var c = GetConversation();
            Debug.Log("NPCと接近!");


            //if (c != null)
            //{
            //    var ev = Schedule.Add<Events.ShowConversation>();
            //    ev.conversation = c;
            //    ev.npc = this;
            //    ev.gameObject = gameObject;
            //    ev.conversationItemKey = "";
            //}
        }

        //public void CompleteQuest(Quest q)
        //{
        //    if (activeQuest != q) throw new System.Exception("Completed quest is not the active quest.");
        //    foreach (var i in activeQuest.requiredItems)
        //    {
        //        model.RemoveInventoryItem(i.item, i.count);
        //    }
        //    activeQuest.RewardItemsToPlayer();
        //    activeQuest.OnFinishQuest();
        //    activeQuest = null;
        //}

        //public void StartQuest(Quest q)
        //{
        //    if (activeQuest != null) throw new System.Exception("Only one quest should be active.");
        //    activeQuest = q;
        //}
        
        //ConversationScript GetConversation()
        //{
        //    if (activeQuest == null)
        //        return conversations[0];
        //    foreach (var q in quests)
        //    {
        //        if (q == activeQuest)
        //        {
        //            if (q.IsQuestComplete())
        //            {
        //                CompleteQuest(q);
        //                return q.questCompletedConversation;
        //            }
        //            return q.questInProgressConversation;
        //        }
        //    }
        //    return null;
        //}
    }
}