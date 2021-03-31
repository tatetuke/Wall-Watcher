using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 現在のシーンからPlayerを見つけ、返すクラス
/// </summary>
public class PlayerProvider : SingletonMonoBehaviour<PlayerProvider>
{
    Player m_player;
    public Player GetPlayer()
    {
        if (m_player == null)
        {
            m_player= FindObjectOfType<Player>();
        }
        if (m_player == null)
        {
            Debug.LogError("player not found");
            return null;
        }
        return m_player;
    }
}
