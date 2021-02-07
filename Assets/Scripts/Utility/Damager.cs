using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Damageableに衝突したとき、healthを減らすクラス
//alwaysAttackがtrueでないとAttack()が実行している最中でないとあたってもダメージが入らないので注意！
public class Damager : MonoBehaviour
{
    public enum DamageType
    {
        always,//接触している間は継続してダメージが入る(クールタイムはダメージを受ける側で)
        once,//起動してから接触したとき1回だけダメージが入る(リセット可)
        delay,//接触が終わったあとも継続してダメージが入る(未実装)
    }

    [SerializeField] int initialDamage=0;//与えるダメージ
    public DamageType type;

    public bool ignoreSameTag = true;//自身と同じタグのDamageableにダメージを与えない
    bool m_activeFlag = true;

    public int Damage
    {
        get
        {
            return (int)m_damageParam.GetValue();
        }
        set
        {
            m_damageParam.SetValue(value);
        }
    }

    public Collider2D damagerCollider;//Damagerの有効範囲

    [System.Serializable]
    public class DamagerEvent : UnityEvent<Damager, Damageable> { }//DamagerがDamageableと衝突したとき実行するイベント
    public DamagerEvent OnDamageableHit { get; } = new DamagerEvent();//Damageableにあたったとき

    HashSet<Damageable> m_damageObj=new HashSet<Damageable>();//typeがonceのときにダメージをすでに与えたオブジェクトを格納
    
    [SerializeField]CharactorParamManager m_manager=default;
    ParameterInt m_damageParam = new ParameterInt();

    private void Start()
    {
        m_manager.AddParam("damage", m_damageParam);
        m_damageParam.SetValue(initialDamage);
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_activeFlag) return;
        switch (type)
        {
            case DamageType.always:
                Attack();
                break;
            case DamageType.once:
                Attack();
                break;
            case DamageType.delay:
                break;
            default:
                break;
        }
    }
    public void SetActive(bool flag) { m_activeFlag = flag; }

    /// <summary>
    /// typeがonceのときのメモリをリセットする
    /// </summary>
    public void ResetMem() { m_damageObj.Clear(); }

    Collider2D[] hits = new Collider2D[15];
    public void Attack()
    {
        int count = damagerCollider.OverlapCollider(new ContactFilter2D(), hits);

        for (int i = 0; i < count; i++)
        {
            // m_LastHit = m_AttackOverlapResults[i];
            Damageable damageable = hits[i].GetComponent<Damageable>();

            if (
                damageable == null || damageable.gameObject == gameObject ||//自分自身にダメージを与えるのを防止
                 (ignoreSameTag && CompareTag(damageable.gameObject.tag))//フラグがtrueかつ自身とタグが同じなら無視
                 ) continue;
            if (!damageable.CanTakeDamage) continue;
            if (type == DamageType.once)
            {
                if (m_damageObj.Contains(damageable)) continue;
                m_damageObj.Add(damageable);
            }
            OnDamageableHit.Invoke(this, damageable);
            damageable.TakeDamage(this);//ダメージをdamageableに与える
        }
    }
}
