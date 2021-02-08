using UnityEngine;
using System.Collections;

//指定矩形範囲内にCheckableがあるかどうかをチェックする
//もしあれば、そのCheckableのイベントを実行させる
//親にRigitbody2Dがあるとうまく動かないときがある。なぜ？
public class Checker : MonoBehaviour
{
    [SerializeField] Collider2D m_collider = default;
    Checkable currentCheckable = null;
    // Use this for initialization

    public string targetTag;

    /// <summary>
    /// エリア内にCheckableがあるか
    /// </summary>
    /// <param name="player"></param>
    /// <returns>true : Checkableがある false : ない</returns>
    public bool HasCheckable() { return  currentCheckable != null; }

    /// <summary>
    /// 範囲内にあるCheckableを１つチェックする
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public bool Check(GameObject player)
    {
        if (currentCheckable != null)
        {
            // moveable = CanMove();
            currentCheckable.TakeCheck(this);
            return true;
        }
        return false;
    }

    int GetCollider2Ds(Vector2 position, Collider2D[] results)
    {
        return m_collider.OverlapCollider(new ContactFilter2D(), results);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentCheckable != null) return;
        Checkable checkable;
        if (collision.TryGetComponent<Checkable>(out checkable))
        {
            currentCheckable = checkable;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (currentCheckable == null) return;
        if (collision.TryGetComponent<Checkable>(out _))
        {
            currentCheckable = null;
        }
    }

    public void SetAngle(float angleDeg)
    {
        transform.rotation = Quaternion.Euler(0, 0, angleDeg + 90f);
    }
}
