using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HakaiResultManager : MonoBehaviour
{
    public HakaiResultLoot[] loot;
    public HakaiScoreItem[] scores;
    public Text scoreText;
    [HideInInspector]public int lootIdx=0;
    [HideInInspector]public int scoreIdx=0;
    public MingameHAKAIGetItemManager itemManager;
    public MinGameHakaiManager2 manager;
    public HakaiSoundManager soundManager;
    public HakaiScoreManager scoreManager;
    public GameObject resultUI;

    public GameObject SObj;
    public GameObject AObj;
    public GameObject BObj;
    public GameObject CObj;
    public GameObject DObj;

    private bool isSkipShowLoot = false;

  

    public IEnumerator ShowResult()
    {
        resultUI.SetActive(true);
        scoreManager.Eval();

        yield return new WaitForSeconds(1f);


        //戦利品を表示
        foreach (GameObject res in itemManager.Item)
        {
            MinGameHAKAIItem itemData = res.GetComponent<MinGameHAKAIItem>();
            //取得できないならパス
            if (!itemData.CanGetItem) continue;
            //インベントリに追加
            manager.inventory.AddItem(itemData.itemSO);

            ShowLoot(itemData.itemSO);
            if (isSkipShowLoot) continue;

            yield return new WaitForSeconds(0.5f);

        }

        yield return new WaitForSeconds(0.5f);

        //スコアを表示
        foreach (HakaiScoreManager.ScoreData score in scoreManager.Scores)
        {
            if (!score.canGetScore) continue;

            ShowScore(score);
            yield return new WaitForSeconds(0.03f);


        }

        yield return new WaitForSeconds(0.8f);
        scoreText.text = ((int)scoreManager.score).ToString();
        scoreText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        //評定を表示
        if (scoreManager.score < 2000)
        {
            DObj.SetActive(true);
        }else if(scoreManager.score < 3000)
        {
            CObj.SetActive(true);

        }
        else if (scoreManager.score < 3000)
        {
            BObj.SetActive(true);
        }
        else if (scoreManager.score < 4000)
        {
            AObj.SetActive(true);
        }
        else
        {
            AObj.SetActive(true);
        }

        //入力待ち
        while (!Input.anyKeyDown)
        {
            yield return null;
        }



        yield return 0;
    }
    private void ShowScore(HakaiScoreManager.ScoreData item)
    {
        if (scoreIdx >= scores.Length)
        {
            Debug.LogError("Result:アイテムに対してUIの枠が不足しています");
            return;
        }

        scores[scoreIdx].bonuceName.text = item.BonusName;

        if (item.type == HakaiScoreManager.SCORE_TYPE.MULTIPLYER)
        {
            scores[scoreIdx].piont.text = "x"+ item.score.ToString();
        }
        else
        {
            scores[scoreIdx].piont.text = item.score.ToString();
        }
        //アクティブ化
        scores[scoreIdx].gameObject.SetActive(true);
        scoreIdx++;
    }



    /// <summary>
    /// 獲得したアイテムを表示する
    /// </summary>
    /// <param name="item">itemso</param>
   private void ShowLoot(ItemSO item)
    {
        if (lootIdx >=loot.Length)
        {
            Debug.LogError("Result:アイテムに対してUIの枠が不足しています");
            return;
        }
        //アイコンを更新
        loot[lootIdx].itemIcon.sprite = item.icon;
        //名前を更新
        loot[lootIdx].itemName.text = item.item_name;

        switch (item.type)
        {
            case ItemSO.Rarelity.N:
                loot[lootIdx].rarerityN.SetActive(true);
                break;
            case ItemSO.Rarelity.R:
                loot[lootIdx].rarerityR.SetActive(true);
                break;
            case ItemSO.Rarelity.SR:
                loot[lootIdx].rareritySR.SetActive(true);
                break;
            case ItemSO.Rarelity.SSR:
                loot[lootIdx].rareritySSR.SetActive(true);
                break;
        }

        //アクティブ化
        loot[lootIdx].gameObject.SetActive(true);

        lootIdx++;
    }
}
