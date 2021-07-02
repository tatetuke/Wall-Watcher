using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGamePaintStatus : MonoBehaviour
{
    public float life;
    public float maxLife;
    protected MiniGamePaintStatusGauge statusGage;
    private void Start()
    {
        if (maxLife == 0)
        {
            maxLife = 100;
        }
        life = maxLife;
        statusGage = GameObject.FindObjectOfType<MiniGamePaintStatusGauge>();
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
