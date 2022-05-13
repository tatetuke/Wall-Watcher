/*
 * マップ移動に関する列挙体のリスト
 * 
 * 
 */


namespace map {
    // マップ番号
    public enum MAP_NUM
    {
        MainMap1F_Floor,
        MainMap2F_Floor,
        MainMap3F_Floor,
        MainMap2F_MainRoom1,
        MainMap2F_MainRoom2,
        MainMap2F_MainRoom3,
        MainMap3_Room1,
        MainMap3_Room2,
        MainMap3F_Renraku1,
        MainMap3F_Renraku2,
        MainMap3_Bokujou,
        MainMap3_Hekimen,
        invalid
    }

    public enum Direction2D
    {
        Up,
        Right,
        Left,
        Down,
        Invalid
    }

    public enum Position2D
    {
        Up,
        Right,
        Left,
        Down,
        Invalid
    }
}
