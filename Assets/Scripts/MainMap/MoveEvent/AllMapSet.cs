using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllMapSet : MonoBehaviour
{
    public static int prevMap = 0;
    public static int currentMap = 0;
    public static int autoWalkingDirection = 0; // 0:左, 1:右
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
        { (-53, -2.15f), (53, -2.15f) },
        {(-72, -1.86f), (72, -1.86f) },
    };
}
