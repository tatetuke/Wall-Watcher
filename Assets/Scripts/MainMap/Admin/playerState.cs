/**
 *
 *  プレイヤー状態管理
 *   いずれ使用予定
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using player;


public class playerState : MonoBehaviour
{
    public static PLAYER_AUTO_WALKING player_auto_direction = PLAYER_AUTO_WALKING.INVALID;  // プレイヤーが自動移動する場合の方向
    public static float auto_move_time = 0.0f;  // プレイヤーが自動移動する時間
}