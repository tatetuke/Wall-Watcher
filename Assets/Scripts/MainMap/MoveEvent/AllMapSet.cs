using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllMapSet : MonoBehaviour
{
    public static int prevMap = 0;
    public static int currentMap = 0;
    public static (float, float, string)[,] warpMap = new (float, float, string)[,] {
        { (0, -2.15f, "MainMap3_Room1"), (-6.5f, -2.8f, "MainMap3_Room1"),(-8.6f,-1.6f,"MainMap3_Wall-TowerWay_1"),(-8.6f,-1.6f,"MainMap3_Wall-TowerWay_2"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None") },
        { (-26, -2.15f, "MainMap3_CircleWay"), (-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None"),(-7, -5, "MainMap3_Room2"),(-1, -1, "None"),(-1, -1, "None") },
        { (30,-2.15f,"MainMap3_CircleWay"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None"),(-25,-3,"MainMap3_Central_Runch"),(0,-5,"MainMap3_Wall")},
        { (-37,-2.15f,"MainMap3_CircleWay"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None")},
        { (-1, -1, "None"),(10, -3, "MainMap3_Room1"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None")},
        { (-1, -1, "None"),(-1,-1,"None"),(0, -1, "MainMap3_Wall-TowerWay_1"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None")},
        { (-1, -1, "None"),(-1,-1,"None"),(8, -1, "MainMap3_Wall-TowerWay_1"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None"),(-1, -1, "None")},
    };
    public static (float, float)[,] warpSameMapPosition = new (float, float)[,] {
        { (-53, -2.15f), (53, -2.15f) },
    };
}
