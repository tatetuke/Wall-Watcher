using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ゲーム内パラメータに一元でアクセスできるクラス
/// </summary>
[DisallowMultipleComponent]
sealed public class GamePropertyManager : SingletonMonoBehaviour<GamePropertyManager>
{
    [System.Serializable]
    public class Property
    {
        PropertyGetter getter;
        public Property( PropertyGetter getter_)
        {
            getter = getter_;
        }
        
        public delegate object PropertyGetter();
        public object Get()
        {
            return getter();
        }
    }

    Dictionary<string, Property> m_properties = new Dictionary<string, Property>();
    //Dictionary<string, string> m_typeProperties = new Dictionary<string, Property<float>>();

    /// <summary>
    /// ゲーム内パラメータ(int)を登録
    /// 登録されたパラメータは自動的にセーブされる。
    /// ゲーム開始時に同じstring keyにアクセスすればセーブされたものが読み込める
    /// </summary>
    /// <param name="key"></param>
    /// <param name="setter"></param>
    /// <param name="getter"></param>
    public void RegisterParam(string key,  Property.PropertyGetter getter)
    {
        if (m_properties.ContainsKey(key))
        {
            Debug.LogWarning($"key '{key}' already exists");
            return;
        }
        m_properties.Add(key, new Property(getter));
    }
    public T GetProperty<T>(string key)
    {
        if (!m_properties.ContainsKey(key))
        {
            Debug.LogError($"key '{key}' does not exists");
            return default;
        }
        return (T)m_properties[key].Get();
    }
}
