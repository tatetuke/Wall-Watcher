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
        public delegate void PropertySetter(T value);
        public delegate T PropertyGetter();
        PropertySetter setter;
        PropertyGetter getter;
        public Property(PropertySetter setter_, PropertyGetter getter_)
        {
            setter = setter_;
            getter = getter_;
        }
        public T Value
        {
            get
            {
                return getter();
            }
            set
            {
                setter(value);
            }
        }
    }

    Dictionary<string, ParameterInt> intParams = new Dictionary<string, ParameterInt>();
    Dictionary<string, ParameterFloat> floatParams = new Dictionary<string, ParameterFloat>();
    Dictionary<string, ParameterBool> boolParams = new Dictionary<string, ParameterBool>();
    Dictionary<string, Property<int>> intProperties = new Dictionary<string, Property<int>>();
    Dictionary<string, Property<float>> floatProperties = new Dictionary<string, Property<float>>();
    Dictionary<string, Property<bool>> boolProperties = new Dictionary<string, Property<bool>>();

    [Header("Property")]
    [SerializeField] string directory = "Data";
    [SerializeField] string filename="propertydata.csv";

    /// <summary>
    /// RegisterParamした後に、そのパラメータをcsvファイルからロードした値で上書きする
    /// </summary>
    public void LoadProperty()
    {
        Debug.Log("Load properties",gameObject);
       var list= CSVReader.Read(directory, filename);
        if (list == null)
        {
            Debug.LogWarning("Property file not found", gameObject);
            return;
        }
        foreach (var obj in list)
        {
            switch (obj[0])
            {
                case "param.int":
                    intParams[obj[1]].Value = int.Parse(obj[2]);
                    break;
                case "param.float":
                    floatParams[obj[1]].Value = float.Parse(obj[2]);
                    break;
                case "param.bool":
                    boolParams[obj[1]].Value = bool.Parse(obj[2]);
                    break;
                case "property.int":
                    intProperties[obj[1]].Value = int.Parse(obj[2]);
                    break;
                case "property.float":
                    floatProperties[obj[1]].Value = float.Parse(obj[2]);
                    break;
                case "property.bool":
                    boolProperties[obj[1]].Value = bool.Parse(obj[2]);
                    break;
                default:
                    break;
            }
        }
    }

    public void SaveProperty()
    {
        Debug.Log("Save properties", gameObject);
        var list = new List<List<string>>();
        foreach (var obj in intParams)
        {
            var l = new List<string>();
            l.Add("param.int");
            l.Add(obj.Key);
            l.Add(obj.Value.Value.ToString());
            list.Add(l);
        }
        foreach (var obj in floatParams)
        {
            var l = new List<string>();
            l.Add("param.float");
            l.Add(obj.Key);
            l.Add(obj.Value.Value.ToString());
            list.Add(l);
        }
        foreach (var obj in boolParams)
        {
            var l = new List<string>();
            l.Add("param.bool");
            l.Add(obj.Key);
            l.Add(obj.Value.Value.ToString());
            list.Add(l);
        }
        foreach (var obj in intProperties)
        {
            var l = new List<string>();
            l.Add("property.int");
            l.Add(obj.Key);
            l.Add(obj.Value.Value.ToString());
            list.Add(l);
        }
        foreach (var obj in floatProperties)
        {
            var l = new List<string>();
            l.Add("property.float");
            l.Add(obj.Key);
            l.Add(obj.Value.Value.ToString());
            list.Add(l);
        }
        foreach (var obj in boolProperties)
        {
            var l = new List<string>();
            l.Add("property.bool");
            l.Add(obj.Key);
            l.Add(obj.Value.Value.ToString());
            list.Add(l);
        }
        CSVReader.Write(directory, filename,list);
    }

    /// <summary>
    /// ゲーム内パラメータ(int)を登録
    /// 登録されたパラメータは自動的にセーブされる。
    /// ゲーム開始時に同じstring keyにアクセスすればセーブされたものが読み込める
    /// </summary>
    /// <param name="key"></param>
    /// <param name="setter"></param>
    /// <param name="getter"></param>
    public void RegisterParam(string key, Property<int>.PropertySetter setter, Property<int>.PropertyGetter getter)
    {
        if (intProperties.ContainsKey(key))
        {
            Debug.LogWarning($"key '{key}' already exists");
            return;
        }
        intProperties.Add(key, new Property<int>(setter, getter));
    }
    /// <summary>
    /// ゲーム内パラメータ(float)を登録
    /// 登録されたパラメータは自動的にセーブされる。
    /// ゲーム開始時に同じstring keyにアクセスすればセーブされたものが読み込める
    /// </summary>
    /// <param name="key"></param>
    /// <param name="setter"></param>
    /// <param name="getter"></param>
    public void RegisterParam(string key, Property<float>.PropertySetter setter, Property<float>.PropertyGetter getter)
    {
        if (floatProperties.ContainsKey(key))
        {
            Debug.LogWarning($"key '{key}' already exists");
            return;
        }
        floatProperties.Add(key, new Property<float>(setter, getter));
    }
    /// <summary>
    /// ゲーム内パラメータ(bool)を登録
    /// 登録されたパラメータは自動的にセーブされる。
    /// ゲーム開始時に同じstring keyにアクセスすればセーブされたものが読み込める
    /// </summary>
    /// <param name="key"></param>
    /// <param name="setter"></param>
    /// <param name="getter"></param>
    public void RegisterParam(string key, Property<bool>.PropertySetter setter, Property<bool>.PropertyGetter getter)
    {
        if (boolProperties.ContainsKey(key))
        {
            Debug.LogWarning($"key '{key}' already exists");
            return;
        }
        boolProperties.Add(key, new Property<bool>(setter, getter));
    }
    public void RegisterParam(string key, ParameterInt param)
    {
        if (intParams.ContainsKey(key))
        {
            Debug.LogWarning($"key '{key}' already exists");
            return;
        }
        intParams.Add(key, param);
    }
    public void RegisterParam(string key, ParameterFloat param)
    {
        if (floatParams.ContainsKey(key))
        {
            Debug.LogWarning($"key '{key}' already exists");
            return;
        }
        floatParams.Add(key, param);
    }
    public void RegisterParam(string key, ParameterBool param)
    {
        if (boolParams.ContainsKey(key))
        {
            Debug.LogWarning($"key '{key}' already exists");
            return;
        }
        boolParams.Add(key, param);
    }
    public ParameterInt GetInt(string key)
    {
        if (!intParams.ContainsKey(key))
        {
            Debug.LogError($"key '{key}' does not exists");
            return null;
        }
        return intParams[key];
    }
    public ParameterFloat GetFloat(string key)
    {
        if (!floatParams.ContainsKey(key))
        {
            Debug.LogError($"key '{key}' does not exists");
            return null;
        }
        return floatParams[key];
    }
    public ParameterBool GetBool(string key)
    {
        if (!boolParams.ContainsKey(key))
        {
            Debug.LogError($"key '{key}' does not exists");
            return null;
        }
        return boolParams[key];
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
    public void SetIntProperty(string key, int value)
    {
        if (!intProperties.ContainsKey(key))
        {
            Debug.LogError($"key '{key}' does not exists");
            return;
        }
        intProperties[key].Value = value;
    }
    public void SetFloatProperty(string key, float value)
    {
        if (!floatProperties.ContainsKey(key))
        {
            Debug.LogError($"key '{key}' does not exists");
            return;
        }
        floatProperties[key].Value = value;
    }
    public void SetBoolProperty(string key, bool value)
    {
        if (!boolProperties.ContainsKey(key))
        {
            Debug.LogError($"key '{key}' does not exists");
            return;
        }
        boolProperties[key].Value = value;
    }
}
