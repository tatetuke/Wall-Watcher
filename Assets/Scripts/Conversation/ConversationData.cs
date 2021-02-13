using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGM.Gameplay;
using System;

/// <summary>
/// ConversationDataManagerで自動的に読み込まれる
/// IDはファイル名(this.name)
/// </summary>
[CreateAssetMenu(menuName = "CreateConversationData", fileName = "ConversationData")]
public class ConversationData : ScriptableObject
{
    [HideInInspector] [SerializeField] public List<Conversations> items = new List<Conversations>();

    //public TalkerData m_left;
    //public TalkerData m_right;

    [HideInInspector]public  string m_firstConversation="";

    public string GetFirst()
    {
        return m_firstConversation;
    }

    public bool ContainsKey(string id)
    {
        foreach (var i in items)
            if (i.id == id) return true;
        return false;
       // return index.ContainsKey(id);
    }

    public Conversations Get(string id)
    {
        foreach (var i in items)
            if (i.id == id) return i;
        return null;
    }

    /// <summary>
    /// リストに会話を追加する
    /// </summary>
    /// <param name="conversationPiece"></param>
    public void Add(Conversations conversation)
    {
        items.Add(conversation);
    }

    public void Set(Conversations originalConversation, Conversations newConversation)
    {
        if (originalConversation.id != newConversation.id)
        {
            foreach (var i in items)
            {
                //conversationのidが書き換えられたとき、会話リストにある選択肢も連動してidを書き換える
                var options = i.options;
                for (var j = 0; j < options.Count; j++)
                {
                    if (options[j].targetId == originalConversation.id)
                    {
                        var c = options[j];
                        c.targetId = newConversation.id;
                        options[j] = c;
                    }
                }
            }
        }
        for (var i = 0; i < items.Count; i++)
        {
            if (items[i].id == originalConversation.id)
            {
                items[i] = newConversation;
                break;
            }
        }
    }

    /// <summary>
    /// リストにあるidがidの要素を削除する
    /// </summary>
    /// <param name="id"></param>
    public void Delete(string id)
    {
        for (var i = 0; i < items.Count; i++)
        {
            if (items[i].id == id)
            {
                items.RemoveAt(i);
                break;
            }
        }
    }
}
