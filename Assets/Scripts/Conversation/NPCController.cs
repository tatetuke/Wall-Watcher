using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //タグの変更.SearchNearNPCで使われる.
            this.tag = "CanConversationNPC";
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //タグの変更.SearchNearNPCで使われる.
            this.tag = "NPC";
        }
    }

    public int GetConversationIndex()
    {
        int res = Random.value >= 0.5 ? 0 : 1;
        return res;
    }
}
