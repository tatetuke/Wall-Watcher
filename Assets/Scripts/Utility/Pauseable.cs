using UnityEngine;
using System.Collections;

/// <summary>
///このコンポーネントを継承するクラスはポーズ時速度を保存して停止する
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Pauseable : MonoBehaviour
{
    Rigidbody2D m_Rigidbody2D;
    Vector2 tmpVeclocity;
    [SerializeField,ReadOnly]bool pauseFlag;
    public bool isPause { get { return pauseFlag; } }
    // Use this for initialization
    void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Pause()
    {
        if (!pauseFlag)
        {
            pauseFlag = true;
            tmpVeclocity = m_Rigidbody2D.velocity;
        }
        m_Rigidbody2D.velocity = Vector2.zero;
    }

    public void Resume()
    {
        if (pauseFlag) {
            pauseFlag = false;
            m_Rigidbody2D.velocity = tmpVeclocity;
        }
    }

    public void Update()
    {
        /*if (GameManager.Instance.IsPause())
        {
            Pause();
            return;
        }*/
    }

}
