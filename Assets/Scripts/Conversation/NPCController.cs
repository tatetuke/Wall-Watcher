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
        private Conversations CurrentConversation;
        string id;
        string FirstText;
        private bool IsTalk=false;
        private void Start()
        {

            if (ConversationsList.Length == 0) return ;
            FirstText = TextBox.text;
            id =ConversationsList[0].GetFirst();
            CurrentConversation = ConversationsList[0].Get(id);
        }


        //Quest activeQuest = null;

        //Quest[] quests;

        //GameModel model = Schedule.GetModel<GameModel>();

        //void OnEnable()
        //{
        //    quests = gameObject.GetComponentsInChildren<Quest>();
        //}

        private void Update()
        {
            if(IsTalk)Texting();
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                IsTalk = true;
                Debug.Log("NPCと接近!");

            }
            
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {

                IsTalk = false;
                id = ConversationsList[0].GetFirst();
                CurrentConversation = ConversationsList[0].Get(id);
                //元からテキストボックスに入力されていた文字を再度表示
                TextBox.text = FirstText;
                Debug.Log("NPCと離れた!");
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
                Debug.Log("スペースキーが押されました");
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