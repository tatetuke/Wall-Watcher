using UnityEngine;
using System.Collections;

//指定矩形範囲内にCheckableがあるかどうかをチェックする
//もしあれば、そのCheckableのイベントを実行させる
//親にRigitbody2Dがあるとうまく動かないときがある。なぜ？
public class Checker : MonoBehaviour
{
    /// <summary>
    /// この範囲にCheckableがあったらチェックできる
    /// </summary>
    [SerializeField] Collider2D m_collider = default;

    /// <summary>
    /// 現在のCheckable
    /// 複数あったら最後に範囲に入ったオブジェクトに上書きされる
    /// </summary>
    Checkable currentCheckable = null;

    //public string targetTag;

    /// <summary>
    /// エリア内にCheckableがあるか
    /// </summary>
    /// <param name="player"></param>
    /// <returns>true : Checkableがある false : ない</returns>
    public bool HasCheckable() { return  currentCheckable != null; }

    /// <summary>
    /// 範囲内にあるCheckableを１つチェックする
    /// NPCであれば会話が始まったり、機械だったらメニューが開いたり
    /// </summary>
    /// <returns></returns>
    public bool Check()
    {
        if (currentCheckable != null)
        {
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
            currentCheckable.OnEnter.Invoke(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (currentCheckable == null) return;
        if (collision.TryGetComponent<Checkable>(out _))
        {
            currentCheckable = null;
            currentCheckable.OnExit.Invoke(this);
        }
    }

    /*public void SetAngle(float angleDeg)
    {
        transform.rotation = Quaternion.Euler(0, 0, angleDeg + 90f);
    }*/
}
