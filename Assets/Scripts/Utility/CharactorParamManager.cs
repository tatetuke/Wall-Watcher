using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターのパラメータを管理するクラス
/// HPや攻撃力など、キャラクターのパラメータを一つのクラスから取得できる
/// バフなどもここで
/// </summary>
sealed public class CharactorParamManager : MonoBehaviour
{
    Dictionary<string, ParameterBase> m_params = new Dictionary<string, ParameterBase>();

    [Header("Debug")]
    [SerializeField, ReadOnly] List<string> m_paramLavel=new List<string>();
    public void AddParam(string key, ParameterBase param)
    {
        m_paramLavel.Add(key);
        m_params.Add(key, param);
    }

    public void SetBuf(string key, Buf buf)
    {
        if (!m_params.ContainsKey(key))
        {
            Debug.Log($"not contains param '{key}'", gameObject);
            return;
        }
        m_params[key].AddBuf(buf);
    }
    public void RemoveBuf(string key, Buf buf)
    {
        if (!m_params.ContainsKey(key))
        {
            Debug.Log($"not contains param '{key}'", gameObject);
            return;
        }
        m_paramLavel.Remove(key);
        m_params[key].RemoveBuf(buf);
    }
    public void ClearBuf(string key)
    {
        if (!m_params.ContainsKey(key))
        {
            Debug.Log($"not contains param '{key}'", gameObject);
            return;
        }
        m_paramLavel.Clear();
        m_params[key].ClearBuf();
    }
    // このパラメータに恒常的に変える（HP最大値など）
    public void SetParam(string key, object setParam)
    {
        if (!m_params.ContainsKey(key))
        {
            Debug.Log($"not contains param '{key}'", gameObject);
            return;
        }
        m_params[key].SetValue(setParam);
    }

    // このパラメータに恒常的に変える（HP最大値など）
    //相対的
    public void SetParamRelative(string key, object addParam)
    {
        if (!m_params.ContainsKey(key))
        {
            Debug.Log($"not contains param '{key}'", gameObject);
            return;
        }
        m_params[key].SetValueRelative(addParam);
    }


}
