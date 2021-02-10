using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [System.Serializable]public class DamageableEvent : UnityEvent<int> { }
    [SerializeField] CharactorParamManager m_manager=default;

    [Header("Health Option")]
    [SerializeField] int m_initialMaxHealth=0;
    [SerializeField] int m_initialHealth=0;//デフォルトのHP
    [SerializeField] float coolTime=0.0f;//ダメージを受けてからの一定の無敵時間

    public int MaxHealth
    {
        get { return (int)m_MaxHealthParam.GetValue(); }
        set { m_MaxHealthParam.SetValue(Mathf.Max(0,value)); }
    }
    public int InitialHealth
    {
        get { return m_initialHealth; }
        set { m_initialHealth = Mathf.Clamp(value, 0, MaxHealth); }
    }
    public int Health
    {
        get {return (int)m_HealthParam.GetValue(); }
        set { m_HealthParam.SetValue(Mathf.Clamp(value, 0, MaxHealth)); }
    }
    public float NormalizedHealth
    {
        get { return (float)Health / (float)MaxHealth; }
    }
    [Header("Property Debug")]
    [SerializeField,ReadOnly] bool m_canTakeDamage=true;//ダメージを受け付けるかどうか
    [SerializeField,ReadOnly] float m_damagedTime=0f;//ダメージを受けてからどれくらい経ったか

    ParameterInt m_HealthParam = new ParameterInt();
    ParameterInt m_MaxHealthParam = new ParameterInt();

    public bool CanTakeDamage
    {
        get { return m_canTakeDamage; }
        private set { m_canTakeDamage = value; }
    }

    // [Header("Callback functions")]
    public UnityEvent OnDead { get; } = new UnityEvent();
    public DamageableEvent OnTakeDamage { get; } = new DamageableEvent();//そのとき受けたダメージを引数に入れる
    public DamageableEvent OnTakeHeal { get; } = new DamageableEvent();//そのとき受けたダメージを引数に入れる


    [System.NonSerialized] public bool activeFlag=true;

    private void Awake()
    {
        m_damagedTime = 0f;
        CanTakeDamage = true;
    }
    private void Start()
    {
        Health = InitialHealth;
        MaxHealth = m_initialMaxHealth;
        m_HealthParam.SetValue(InitialHealth);
        m_manager.AddParam("HP", m_HealthParam);
        m_manager.AddParam("HPMax", m_MaxHealthParam);
    }


    public void Initialize(int initialHealth,int maxHealth,float coolTime_)
    {
        MaxHealth = maxHealth;
        InitialHealth = initialHealth;
        Health = initialHealth;
        coolTime = coolTime_;
    }

    // Update is called once per frame
    void Update()
    {
        //無敵時間
        if (!CanTakeDamage)
        {
            m_damagedTime += Time.deltaTime;
            if (m_damagedTime > coolTime)
            {
                CanTakeDamage = true;
                m_damagedTime = 0f;
            }
        }
    }

    public void TakeDamage(Damager damager)//ダメージを受ける
    {
        if (!activeFlag || !CanTakeDamage || IsDead()) return;
        Debug.Log("take damage:" + damager.Damage, gameObject);
        Health -= damager.Damage;
        OnTakeDamage.Invoke(damager.Damage);
        if (IsDead())//もしHPが0以下になったら死んだときのイベントを呼び出す
        {
            OnDead.Invoke();
        }
        CanTakeDamage = false;
    }

    public void TakeHeal(int health)
    {
        if (!activeFlag || IsDead()) return;
        Health += health;
        OnTakeHeal.Invoke(health);
    }

    public bool IsDead() { return Health == 0; }


}
