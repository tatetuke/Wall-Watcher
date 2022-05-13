using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kyoichi;
using System;
public class MinGameHakaiToolData : MonoBehaviour
{
    [HideInInspector] public MinGameHakaiToolStatus[] Tools = new MinGameHakaiToolStatus[3];
    public Sprite[] ToolImages;
    private Inventry inventry;
    private ItemManager itemManager;
    /// <summary>
    /// レベル1から順に入れる必要がある。
    /// </summary>
    public List<ItemSO> tool1;
    public List<ItemSO> tool2;
    public List<ItemSO> tool3;

    private void Start()
    {
        
        inventry = GameObject.Find("Managers").GetComponent<Inventry>();
        itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();

        for (int i = 0; i < Tools.Length; i++)
        {
            Tools[i] = new MinGameHakaiToolStatus();
        }
        
        ///ツールの初期化
        ///インベントリのロードを待つために時間差で処理している
        StartCoroutine(InitCoruitine());
     
    }
    IEnumerator InitCoruitine()
    {
        int t = 0;
        while (t<30)
        {
            yield return null;

            t++;
        }
        //inventry.LoadFromFile();
        Tool1Init();
        Tool2Init();
        Tool3Init();
        yield return 0;
    }

    //ToDo
    //アイテムのレベルに関しては今後プレイヤーの持っているアイテムの情報から取得する。
    private void Tool1Init()
    {
        
        if (inventry.HasItem(tool1[0]))
        {
            Tool1InitLv1();
        }
        else if (inventry.HasItem(tool1[1]))
        {
            Tool1InitLv2();

        }
        else if (inventry.HasItem(tool1[2]))
        {
            Tool1InitLv3();
        }
        else
        {

            Debug.LogError("inventryにツール1が存在しません。\nツール1の初期化に失敗しました。\nツール1レベル1をinventryに追加します。");

            inventry.AddItem(tool1[0]);
            Tool1InitLv1();
            //inventry.SaveToFile();

        }
    }

    private void Tool2Init()
    {

        if (inventry.HasItem(tool2[0]))
        {
            Tool2InitLv1();
        }
        else if (inventry.HasItem(tool2[1]))
        {
            Tool2InitLv2();

        }
        else if (inventry.HasItem(tool2[2]))
        {
            Tool2InitLv3();
        }
        else
        {
            Debug.LogError("inventryにツール2が存在しません。\nツール2の初期化に失敗しました。\nツール2レベル1をinventryに追加します。");
            inventry.AddItem(tool2[0]);
            Tool2InitLv1();
            //inventry.SaveToFile();

        }
    }

    private void Tool3Init()
    {
        if (inventry.HasItem(tool3[0]))
        {
            Tool3InitLv1();
        }
        else if (inventry.HasItem(tool3[1]))
        {
            Tool3InitLv2();

        }
        else if (inventry.HasItem(tool3[2]))
        {
            Tool3InitLv3();
        }
        else
        {
            Debug.LogError("inventryにツール3が存在しません。\nツール2の初期化に失敗しました。\nツール2レベル1をinventryに追加します。");
            inventry.AddItem(tool3[0]);
            Tool3InitLv1();
            //inventry.SaveToFile();
        }
    }


    ///////////////
    //  一つ目のアイテム
    /////////////////
    ///
    /// 
    /// 
    /// <summary>
    /// ツールの情報をここに書き込む
    /// </summary>
    private void Tool1InitLv1()
    {
        Tools[0].toolName = "アルパカ";
        Tools[0].discription = "もふもふな生き物。\n威嚇で吐く唾はめちゃめちゃ臭い。" +
            "\nクリックしたマスとその周囲8マスを裏返す。LV1のテキスト";
        Tools[0].damage.Add(10);
        Tools[0].damage.Add(9);
        Tools[0].damage.Add(8);
        //クリックした周囲8マス
        Tools[0].HanniImage = ToolImages[0];
        Tools[0].level = 1;

        Tools[0].CanChangeSprite = new int[9] { 1,1,1,1,1,1,1,1,1};

    }
    private void Tool1InitLv2()
    {
        Tools[0].toolName = "アルパカ";
        Tools[0].discription = "もふもふな生き物。\n威嚇で吐く唾はめちゃめちゃ臭い。" +
            "\nクリックしたマスとその周囲8マスを裏返す。LV2のテキスト";
        Tools[0].damage.Add(10);
        Tools[0].damage.Add(9);
        Tools[0].damage.Add(8);
        //クリックした周囲8マス
        Tools[0].HanniImage = ToolImages[0];
        Tools[0].level = 2;

        Tools[0].CanChangeSprite = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };

    }
    private void Tool1InitLv3()
    {
        Tools[0].toolName = "アルパカ";
        Tools[0].discription = "もふもふな生き物。\n威嚇で吐く唾はめちゃめちゃ臭い。" +
            "\nクリックしたマスとその周囲8マスを裏返す。LV3のテキスト";
        Tools[0].damage.Add(10);
        Tools[0].damage.Add(9);
        Tools[0].damage.Add(8);
        //クリックした周囲8マス
        Tools[0].HanniImage = ToolImages[0];
        Tools[0].level = 3;

        Tools[0].CanChangeSprite = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };

    }


    ///
    /// 二つ目のアイテム
    ///



    private void Tool2InitLv1()
    {
        Tools[1].toolName = "オリックス";
        Tools[1].discription = "ながくて鋭い角がかっこいい！\n実はウシ科に属する。" +
            "\nクリックしたマスとその上下左右4マスを裏返す。Lv1のテキスト";
        Tools[1].damage.Add(30);
        Tools[1].damage.Add(20);
        Tools[1].damage.Add(15);
        //クリックした周囲8マス
        Tools[1].HanniImage = ToolImages[1];
        Tools[1].level = 1;
        Tools[1].CanChangeSprite = new int[9] { 0, 1, 0, 1, 2, 1, 0, 1, 0 };


    }
    private void Tool2InitLv2()
    {
        Tools[1].toolName = "オリックス";
        Tools[1].discription = "ながくて鋭い角がかっこいい！\n実はウシ科に属する。" +
            "\nクリックしたマスとその上下左右4マスを裏返す。Lv2のテキスト";
        Tools[1].damage.Add(30);
        Tools[1].damage.Add(20);
        Tools[1].damage.Add(15);
        //クリックした周囲8マス
        Tools[1].HanniImage = ToolImages[1];
        Tools[1].level = 2;
        Tools[1].CanChangeSprite = new int[9] { 0, 1, 0, 1, 2, 1, 0, 1, 0 };


    }
    private void Tool2InitLv3()
    {
        Tools[1].toolName = "オリックス";
        Tools[1].discription = "ながくて鋭い角がかっこいい！\n実はウシ科に属する。" +
            "\nクリックしたマスとその上下左右4マスを裏返す。Lv3のテキスト";
        Tools[1].damage.Add(30);
        Tools[1].damage.Add(20);
        Tools[1].damage.Add(15);
        //クリックした周囲8マス
        Tools[1].HanniImage = ToolImages[1];
        Tools[1].level = 3;
        Tools[1].CanChangeSprite = new int[9] { 0, 1, 0, 1, 2, 1, 0, 1, 0 };


    }



    ///
    /// 3つ目のアイテム
    ///

    private void Tool3InitLv1()
    {
        Tools[2].toolName = "チーター";
        Tools[2].discription = "足がとてもはやい。\n狙った獲物は逃がさないサバンナのハンター。" +
            "\nクリックしたマスのみ裏返す。Lv1のテキスト";
        Tools[2].damage.Add(50);
        Tools[2].damage.Add(40);
        Tools[2].damage.Add(35);
        Tools[2].HanniImage = ToolImages[2];
        Tools[2].level = 1;
        //クリックした壁のみ
        Tools[2].CanChangeSprite = new int[9] { 0, 0, 0, 0, 4, 0, 0, 0, 0 };


    }
    private void Tool3InitLv2()
    {
        Tools[2].toolName = "チーター";
        Tools[2].discription = "足がとてもはやい。\n狙った獲物は逃がさないサバンナのハンター。" +
            "\nクリックしたマスのみ裏返す。Lv2のテキスト";
        Tools[2].damage.Add(50);
        Tools[2].damage.Add(40);
        Tools[2].damage.Add(35);
        Tools[2].HanniImage = ToolImages[2];
        Tools[2].level = 2;
        //クリックした壁のみ
        Tools[2].CanChangeSprite = new int[9] { 0, 0, 0, 0, 4, 0, 0, 0, 0 };


    }
    private void Tool3InitLv3()
    {
        Tools[2].toolName = "チーター";
        Tools[2].discription = "足がとてもはやい。\n狙った獲物は逃がさないサバンナのハンター。" +
            "\nクリックしたマスのみ裏返す。Lv3のテキスト";
        Tools[2].damage.Add(50);
        Tools[2].damage.Add(40);
        Tools[2].damage.Add(35);
        Tools[2].HanniImage = ToolImages[2];
        Tools[2].level = 3;
        //クリックした壁のみ
        Tools[2].CanChangeSprite = new int[9] { 0, 0, 0, 0, 4, 0, 0, 0, 0 };


    }


}
