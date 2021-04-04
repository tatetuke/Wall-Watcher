using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinGameHakaiToolStatus : MonoBehaviour
{
    /// <summary>
    /// 道具の名前
    /// </summary>
    public string toolName;
    /// <summary>
    /// 道具の説明
    /// </summary>
    public string discription;
    /// <summary>
    /// 各レベルでの道具を使用して受けるダメージ.呼び出すときはdamage[level-1]。levelが1以上がどうかで場合分けする必要がある。
    /// </summary>
    public List<int> damage=new List<int>();
    /// <summary>
    /// 道具のレベル.0はアイテムを取得していない状態。１～
    /// </summary>
    public int level;
    /// <summary>
    /// 道具の範囲を簡単に表した画像
    /// </summary>
    public Sprite HanniImage;
    /// <summary>
    /// 3x3マスの正方形を左から右に、上から下に見たとき、画像を変更できるかどうか.
    /// </summary>
    public bool[] CanChangeSprite = new bool[9] {true,true,true,true,true,true,true,true,true};
}
