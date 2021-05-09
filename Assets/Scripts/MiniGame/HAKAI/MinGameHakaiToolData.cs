using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinGameHakaiToolData : MonoBehaviour
{
    [HideInInspector] public MinGameHakaiToolStatus[] Tools=new MinGameHakaiToolStatus[3];
    public Sprite[] ToolImages;
    private void Start()
    {
        for(int i = 0; i < Tools.Length; i++)
        {
            Tools[i] = gameObject.AddComponent<MinGameHakaiToolStatus>();
        }
        Tool1Init();
        Tool2Init();
        Tool3Init();
    }
    //ToDo
    //アイテムのレベルに関しては今後プレイヤーの持っているアイテムの情報から取得する。
    private void Tool1Init()
    {
        
        Tools[0].toolName = "アルパカ";
        Tools[0].discription = "もふもふな生き物。\n威嚇で吐く唾はめちゃめちゃ臭い。\nクリックしたマスとその周囲8マスを裏返す。";
        Tools[0].damage.Add(10);
        Tools[0].damage.Add(9);
        Tools[0].damage.Add(8);
        //クリックした周囲8マス
        Tools[0].HanniImage = ToolImages[0];
        Tools[0].level = 1;

    }
    private void Tool2Init()
    {
        Tools[1].toolName = "オリックス";
        Tools[1].discription = "ながくて鋭い角がかっこいい！\n実はウシ科に属する。\nクリックしたマスとその上下左右4マスを裏返す。";
        Tools[1].damage.Add(30);
        Tools[1].damage.Add(20);
        Tools[1].damage.Add(15);
        //クリックした周囲8マス
        Tools[1].HanniImage = ToolImages[1];
        Tools[1].level = 2;
        Tools[1].CanChangeSprite[0] = false;
        Tools[1].CanChangeSprite[2] = false;
        Tools[1].CanChangeSprite[6] = false;
        Tools[1].CanChangeSprite[8] = false;

    }
    private void Tool3Init()
    {
        Tools[2].toolName = "チーター";
        Tools[2].discription = "足がとてもはやい。\n狙った獲物は逃がさないサバンナのハンター。\nクリックしたマスのみ裏返す。";
        Tools[2].damage.Add(50);
        Tools[2].damage.Add(40);
        Tools[2].damage.Add(35);
        Tools[2].HanniImage = ToolImages[2];
        Tools[2].level = 1;
        //クリックした壁のみ
        Tools[2].CanChangeSprite[0] = false;
        Tools[2].CanChangeSprite[1] = false;
        Tools[2].CanChangeSprite[2] = false;
        Tools[2].CanChangeSprite[3] = false;
        Tools[2].CanChangeSprite[5] = false;
        Tools[2].CanChangeSprite[6] = false;
        Tools[2].CanChangeSprite[7] = false;
        Tools[2].CanChangeSprite[8] = false;

    }

}
