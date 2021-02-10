using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ParameterBase
{
    protected List<Buf> m_bufs=new List<Buf>();
    public void AddBuf(Buf buf) { m_bufs.Add(buf); }
    public void RemoveBuf(Buf buf) { m_bufs.Remove(buf); }
    public void ClearBuf() { m_bufs.Clear(); }
    virtual public object GetValue() { return 0; }
    virtual public void SetValue(object value) { }
    virtual public void SetValueRelative(object value) { }
}

sealed public class ParameterInt: ParameterBase
{
    int m_value;

    /// <summary>
    /// get: バフを考慮した値が帰ってくる
    /// set: 実際の値をセット
    /// </summary>
    public int Value
    {
        get
        {
            float a = 1f, b = 0f;
            foreach (var buf in m_bufs)
            {
                a *= buf.GetA();
                b += buf.GetB();
            }
            return (int)(m_value * a + b);
        }
        set
        {
            m_value = value;
        }
    }
     public override object GetValue() {return Value; }
     public override void SetValue(object value) {
        if(!(value is int))
        {
            Debug.Log("value is not int");
            return;
        }
        Value = (int)value;
    }
     public override void SetValueRelative(object value)
    {
        if (!(value is int))
        {
            Debug.Log("value is not int");
            return;
        }
        Value += (int)value;
    }

}

sealed public class ParameterFloat : ParameterBase
{
    float m_value;
    public float Value
    {
        get
        {
            float a = 1f, b = 0f;
            foreach (var buf in m_bufs)
            {
                a *= buf.GetA();
                b += buf.GetB();
            }
            return m_value * a + b;
        }
        set
        {
            m_value = value;
        }
    }
    public override object GetValue() { return Value; }

    public override void SetValue(object value)
    {
        if (!(value is float))
        {
            Debug.Log("value is not float");
            return;
        }
        Value = (float)value;
    }
    public override void SetValueRelative(object value)
    {
        if (!(value is float))
        {
            Debug.Log("value is not float");
            return;
        }
        Value += (float)value;
    }

}

sealed public class ParameterBool : ParameterBase
{
    bool m_value;
    public bool Value
    {
        get
        {
            bool ans = m_value;
            foreach (var buf in m_bufs)
            {
                float a = buf.GetA();
                if (Mathf.Approximately(a, 0))//もしaが0ならbの正負が代入される
                {
                    ans = buf.GetB() >= 0;
                }
                else if (a < 0)//aが負なら反転
                {
                    ans = !ans;
                }
            }
            return ans;
        }
        set
        {
            m_value = value;
        }
    }
    public override object GetValue() { return Value; }

    public override void SetValue(object value)
    {
        if (!(value is bool))
        {
            Debug.Log("value is not bool");
            return;
        }
        Value = (bool)value;
    }

    /// <summary>
    /// falseなら反転
    /// </summary>
    /// <param name="value"></param>
    public override void SetValueRelative(object value)
    {
        if (!(value is bool))
        {
            Debug.Log("value is not float");
            return;
        }
        Value = !(bool)value ^ Value;
    }

}