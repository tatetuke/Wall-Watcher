using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using map;

public class AllMapSet : MonoBehaviour
{
    
    // 今後　static　外す予定
    public static MAP_NUM prevMap = MAP_NUM.invalid;     // 移動前マップ番号
    public static MAP_NUM currentMap = MAP_NUM.invalid;  // 移動後マップ番号
    public static float initial_x = 0, initial_y = 0;   // 移動後座標
    public static Direction2D autoWalkingDirection = Direction2D.Invalid; // 移動後自動移動  0:マップ左, 1:マップ右

    // 移動後のマップ座標データとマップ番号を保存
    public static void update_initial_mapdata(MAP_NUM from, MAP_NUM to, float x, float y)
    {
        prevMap = from;
        currentMap = to;
        initial_x = x;
        initial_y = y;
    }

    /// <summary>
    /// マップ移動
    /// </summary>
    /// <param name='fromNum'>移動前マップ番号</param>
    /// <param name='toNum'>移動先マップ番号</param>
    /// <param name='x'>移動先 x座標</param>
    /// <param name='y'>移動先 y座標</param>
    /// <param name='autoDirection'>移動先で自動的に0.4秒移動するか</param>
    /// <param name='time'>暗転にかかる時間(秒)</param>
    public static void warp_player_position(MAP_NUM fromNum, MAP_NUM toNum, float x, float y, Direction2D autoDirection, float time)
    {
        AllMapSet.update_initial_mapdata(fromNum, toNum, x, y);
        AllMapSet.autoWalkingDirection = autoDirection;
        FadeManager.Instance.LoadLevel(toNum.ToString(), time);
    }


    // 移動後座標取得
    public static (float x, float y) get_initial_position()
    {
        return (initial_x, initial_y);
    }

    public static (float, float, string)[,] warpMapWithElavator = new (float, float, string)[,]
    {
        {(0, 0, "Nnoe"),(0, 0, "Nnoe"),(0, 0, "Nnoe")},
    };
}
