using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MiniGamePaintStatusGauge : MonoBehaviour
{
    [SerializeField]
    private Image GreenGauge;
    [SerializeField]
    private Image RedGauge;

    private MiniGamePaintStatus status;
    private Tween redGaugeTween;

    public void GaugeReduction(float reducationValue, float time = 1f)
    {
        var valueFrom = status.life / status.maxLife;
        var valueTo = (status.life - reducationValue) / status.maxLife;

        // 緑ゲージ減少
        GreenGauge.fillAmount = valueTo;

        if (redGaugeTween != null)
        {
            redGaugeTween.Kill();
        }

        // 赤ゲージ減少
        redGaugeTween = DOTween.To(
            () => valueFrom,
            x => {
                RedGauge.fillAmount = x;
            },
            valueTo,
            time
        );
    }

    public void SetPlayer(MiniGamePaintStatus m_status)
    {
        this.status = m_status;
    }
}