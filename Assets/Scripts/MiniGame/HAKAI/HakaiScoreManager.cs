using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HakaiScoreManager : MonoBehaviour
{
    public MinGameHakaiManager2 gameManager;
    public enum SCORE_TYPE
    {
        MULTIPLYER,
        BONUS
    } 
    public struct ScoreData{
        public SCORE_TYPE type;
        public string BonusName;
        public float score;
        public bool canGetScore;
        public ScoreData(string _name,int _score,SCORE_TYPE _type)
        {
            BonusName = _name;
            canGetScore = true;
            score = _score;
            type = _type;
        }
    }
    
    [HideInInspector]public float score=0;
    [HideInInspector]public float multiply=1;
    [HideInInspector] public ScoreData  excavate= new ScoreData("採掘点", 0, SCORE_TYPE.BONUS);
    [HideInInspector] public ScoreData  itemBonus1= new ScoreData("アイテム発見", 0, SCORE_TYPE.BONUS);
    [HideInInspector] public ScoreData  itemBonus2= new ScoreData("全発見ボーナス", 5000, SCORE_TYPE.BONUS);
    [HideInInspector] public ScoreData speed = new ScoreData("スピード倍率", 0, SCORE_TYPE.MULTIPLYER);
    [HideInInspector] public List<ScoreData> Scores= new List<ScoreData>();
   

    public void Eval()
    {
        EvalSpeed();
        EvalItemBonus1();
        EvalItemBonus2();
        EvalExcavate();

        Scores.Add(excavate);
        Scores.Add(itemBonus1);
        Scores.Add(itemBonus2);
        Scores.Add(speed);

        foreach (ScoreData _item in Scores){
            if (!_item.canGetScore) continue;
            if (_item.type == SCORE_TYPE.BONUS)
            {
                score += _item.score;
            }
            if (_item.type == SCORE_TYPE.MULTIPLYER)
            {
                multiply *= _item.score;
            }

        }
        score *= multiply;

    }
    private void EvalSpeed()
    {

        speed.canGetScore = true;
        speed.score = 1.5f;
        
    }
    private void EvalItemBonus1()
    {

        foreach(GameObject obj in gameManager.itemManager.Item)
        {

            if (!obj.GetComponent<MinGameHAKAIItem>().CanGetItem) continue;
            itemBonus1.score += 500;
        }
        itemBonus1.canGetScore = true;

    }

    private void EvalItemBonus2()
    {
        itemBonus2.canGetScore = true;

        foreach (GameObject obj in gameManager.itemManager.Item)
        {
            if (!obj.GetComponent<MinGameHAKAIItem>().CanGetItem) {
                itemBonus2.canGetScore = false;
                break;
            }
        }

    }
    private void EvalExcavate()
    {

        excavate.canGetScore = true;
        excavate.score = 0;

    }
}
