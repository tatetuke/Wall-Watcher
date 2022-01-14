﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllMapSet : MonoBehaviour
{
    // 今後　static　外す予定
    public static Warp.MAP_NUM prevMap = Warp.MAP_NUM.MainMap3F_Floor;     // 移動前マップ番号
    public static Warp.MAP_NUM currentMap = Warp.MAP_NUM.MainMap3F_Floor;  // 移動後マップ番号
    public static float initial_x = 0, initial_y = 0;   // 移動後座標
    public static int autoWalkingDirection = 0; // 移動後自動移動  0:マップ左, 1:マップ右

    // warpMap[移動前マップ番号][移動後マップ番号] = (移動先 x 座標, 移動先 y 座標, 移動先シーン名)
    public static (float, float, string)[,] warpMap = new (float, float, string)[,] {
        { (0, -2.15f, "None"), (-11, -2.83f, "MainMap3_Room1"),(-8.6f,-1.6f,"MainMap3_Renraku1"),(-8.6f,-1.6f,"MainMap3_Renraku2"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None") },
        { (-26, -2.15f, "MainMap3_Floor"), (0, -2.83f, "None"),(-1, -1, "None"),(-1, -1, "None"),(-10, -2.76f, "MainMap3_Room2"),(-1, -1, "None"),(-1, -1, "None") },
        { (30,-2.15f,"MainMap3_Floor"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None"),(-25,-2.76f,"MainMap3_Bokujou"),(28.6f,-1.86f,"MainMap3_Hekimen")},
        { (-37,-2.15f,"MainMap3_Floor"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None"),(-41.6f, -1.86f, "MainMap3_Hekimen")},
        { (-1, -1, "None"),(11, -2.83f, "MainMap3_Room1"),(-1, -1, "None"),(-1, -1, "None"),(0, -2.76f, "None"),(-1, -1, "None"),(-1, -1, "None")},
        { (-1, -1, "None"),(-1,-1,"None"),(0, -1, "MainMap3_Renraku1"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None"),(0, -2.76f, "None")},
        { (-1, -1, "None"),(-1,-1,"None"),(8, -1, "MainMap3_Renraku1"),(8, -1, "MainMap3_Renraku2"),(-1, -1, "None"),(-1, -1, "None"),(0, -1.86f, "None")},
    };
    public static (float, float)[,] warpSameMapPosition = new (float, float)[,] {
        { (-52, -2.15f), (51, -2.15f) },
        {(-72, -1.86f), (72, -1.86f) },
    };

    // 移動後のマップ座標データとマップ番号を保存
    public static void update_initial_mapdata(Warp.MAP_NUM from, Warp.MAP_NUM to, float x, float y)
    {
        prevMap = from;
        currentMap = to;
        initial_x = x;
        initial_y = y;
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
