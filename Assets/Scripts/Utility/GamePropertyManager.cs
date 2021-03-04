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
    public class Property<T>
    {
        public delegate T PropertyGetter();
        PropertyGetter getter;
        public Property( PropertyGetter getter_)
        {
            getter = getter_;
        }
        public T Value
        {
            get
            {
                return getter();
            }
        }
    }

    Dictionary<string, Property<int>> intProperties = new Dictionary<string, Property<int>>();
    Dictionary<string, Property<float>> floatProperties = new Dictionary<string, Property<float>>();
    Dictionary<string, Property<bool>> boolProperties = new Dictionary<string, Property<bool>>();
    Dictionary<string, Property<Vector2>> vec2Properties = new Dictionary<string, Property<Vector2>>();
    Dictionary<string, Property<Vector3>> vec3Properties = new Dictionary<string, Property<Vector3>>();
    Dictionary<string, Property<string>> stringProperties = new Dictionary<string, Property<string>>();

    /// <summary>
    /// ゲーム内パラメータ(int)を登録
    /// 登録されたパラメータは自動的にセーブされる。
    /// ゲーム開始時に同じstring keyにアクセスすればセーブされたものが読み込める
    /// </summary>
    /// <param name="key"></param>
    /// <param name="setter"></param>
    /// <param name="getter"></param>
    public void RegisterParam(string key,  Property<int>.PropertyGetter getter)
    {
        if (intProperties.ContainsKey(key))
        {
            Debug.LogWarning($"key '{key}' already exists");
            return;
        }
        intProperties.Add(key, new Property<int>( getter));
    }
    /// <summary>
    /// ゲーム内パラメータ(float)を登録
    /// 登録されたパラメータは自動的にセーブされる。
    /// ゲーム開始時に同じstring keyにアクセスすればセーブされたものが読み込める
    /// </summary>
    /// <param name="key"></param>
    /// <param name="setter"></param>
    /// <param name="getter"></param>
    public void RegisterParam(string key, Property<float>.PropertyGetter getter)
    {
        if (floatProperties.ContainsKey(key))
        {
            Debug.LogWarning($"key '{key}' already exists");
            return;
        }
        floatProperties.Add(key, new Property<float>( getter));
    }
    /// <summary>
    /// ゲーム内パラメータ(bool)を登録
    /// 登録されたパラメータは自動的にセーブされる。
    /// ゲーム開始時に同じstring keyにアクセスすればセーブされたものが読み込める
    /// </summary>
    /// <param name="key"></param>
    /// <param name="setter"></param>
    /// <param name="getter"></param>
    public void RegisterParam(string key, Property<bool>.PropertyGetter getter)
    {
        if (boolProperties.ContainsKey(key))
        {
            Debug.LogWarning($"key '{key}' already exists");
            return;
        }
        boolProperties.Add(key, new Property<bool>( getter));
    }
    public void RegisterParam(string key,Property<Vector2>.PropertyGetter getter)
    {
        if (vec2Properties.ContainsKey(key))
        {
            Debug.LogWarning($"key '{key}' already exists");
            return;
        }
        vec2Properties.Add(key, new Property<Vector2>( getter));
    }
    public void RegisterParam(string key, Property<Vector3>.PropertyGetter getter)
    {
        if (vec3Properties.ContainsKey(key))
        {
            Debug.LogWarning($"key '{key}' already exists");
            return;
        }
        vec3Properties.Add(key, new Property<Vector3>( getter));
    }
    public void RegisterParam(string key,  Property<string>.PropertyGetter getter)
    {
        if (stringProperties.ContainsKey(key))
        {
            Debug.LogWarning($"key '{key}' already exists");
            return;
        }
        stringProperties.Add(key, new Property<string>( getter));
    }
    public int GetIntProperty(string key)
    {
        if (!intProperties.ContainsKey(key))
        {
            Debug.LogError($"key '{key}' does not exists");
            return 0;
        }
        return intProperties[key].Value;
    }
    public float GetFloatProperty(string key)
    {
        if (!floatProperties.ContainsKey(key))
        {
            Debug.LogError($"key '{key}' does not exists");
            return float.NaN;
        }
        return floatProperties[key].Value;
    }
    public bool GetBoolProperty(string key)
    {
        if (!boolProperties.ContainsKey(key))
        {
            Debug.LogError($"key '{key}' does not exists");
            return false;
        }
        return boolProperties[key].Value;
    }
    public Vector2 GetVec2Property(string key)
    {
        if (!vec2Properties.ContainsKey(key))
        {
            Debug.LogError($"key '{key}' does not exists");
            return Vector2.zero;
        }
        return vec2Properties[key].Value;
    }
    public Vector3 GetVec3Property(string key)
    {
        if (!vec3Properties.ContainsKey(key))
        {
            Debug.LogError($"key '{key}' does not exists");
            return Vector3.zero;
        }
        return vec3Properties[key].Value;
    }
    public string GetStringProperty(string key)
    {
        if (!stringProperties.ContainsKey(key))
        {
            Debug.LogError($"key '{key}' does not exists");
            return null;
        }
        return stringProperties[key].Value;
    }
}
