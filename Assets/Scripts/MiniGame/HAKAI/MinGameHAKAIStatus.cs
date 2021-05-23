using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinGameHAKAIStatus : MonoBehaviour
{
   public float life;
   public float maxLife;
   protected MingameHAKAIStatusGauge statusGage;
    private void Start()
    {
        if (maxLife == 0)
        {
            maxLife = 100;
        }
        life = maxLife;
        statusGage = GameObject.FindObjectOfType<MingameHAKAIStatusGauge>();
        statusGage.SetPlayer(this);

    }
    /// <summary>
    /// 体力を減らす関数
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void Damage(float damage)
    {
        statusGage.GaugeReduction(damage);
        life -= damage;
    }
}
