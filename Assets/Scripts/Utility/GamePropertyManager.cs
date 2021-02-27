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

    Dictionary<string, Property<int>> intProperties = new Dictionary<string, Property<int>>();
    Dictionary<string, Property<float>> floatProperties = new Dictionary<string, Property<float>>();
    Dictionary<string, Property<bool>> boolProperties = new Dictionary<string, Property<bool>>();
    Dictionary<string, Property<Vector2>> vec2Properties = new Dictionary<string, Property<Vector2>>();
    Dictionary<string, Property<Vector3>> vec3Properties = new Dictionary<string, Property<Vector3>>();
    Dictionary<string, Property<string>> stringProperties = new Dictionary<string, Property<string>>();

    [Header("Property")]
    [SerializeField] string directory = "Data";
    [SerializeField] string filename="propertydata.csv";

    //変数をファイルに保存するとき、その変数がどの型なのかを記述するヘッダ
    const string intHeader = "property.int";
    const string floatHeader = "property.float";
    const string boolHeader = "property.bool";
    const string vec2Header = "property.vec2";
    const string vec3Header = "property.vec3";
    const string stringHeader = "property.string";

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
                case intHeader:
                    intProperties[obj[1]].Value = int.Parse(obj[2]);
                    break;
                case floatHeader:
                    floatProperties[obj[1]].Value = float.Parse(obj[2]);
                    break;
                case boolHeader:
                    boolProperties[obj[1]].Value = bool.Parse(obj[2]);
                    break;
                case vec2Header:
                    float x = float.Parse(obj[2]);
                    float y = float.Parse(obj[3]);
                    vec2Properties[obj[1]].Value = new Vector2(x, y);
                    break;
                case vec3Header:
                    float x2 = float.Parse(obj[2]);
                    float y2 = float.Parse(obj[3]);
                    float z2 = float.Parse(obj[4]);
                    vec3Properties[obj[1]].Value = new Vector3(x2, y2, z2);
                    break;
                case stringHeader:
                    stringProperties[obj[1]].Value = obj[2];
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
        foreach (var obj in intProperties)
        {
            var l = new List<string> {
                intHeader,
                obj.Key,
                obj.Value.Value.ToString()
            };
            list.Add(l);
        }
        foreach (var obj in floatProperties)
        {
            var l = new List<string> {
                floatHeader,
                obj.Key,
                obj.Value.Value.ToString()
            };
            list.Add(l);
        }
        foreach (var obj in boolProperties)
        {
            var l = new List<string> {
                boolHeader,
                obj.Key,
                obj.Value.Value.ToString()
            };
            list.Add(l);
        }
        foreach (var obj in vec2Properties)
        {
            var l = new List<string> {
                vec2Header,
                obj.Key,
                obj.Value.Value.x.ToString(),
                obj.Value.Value.y.ToString()
            };
            list.Add(l);
        }
        foreach (var obj in vec3Properties)
        {
            var l = new List<string> {
                vec3Header,
                obj.Key,
                obj.Value.Value.x.ToString(),
                obj.Value.Value.y.ToString(),
                obj.Value.Value.z.ToString()
            };
            list.Add(l);
        }
        foreach (var obj in stringProperties)
        {
            var l = new List<string> {
                stringHeader,
                obj.Key,
                obj.Value.Value,
            };
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
    public void RegisterParam(string key, Property<Vector2>.PropertySetter setter, Property<Vector2>.PropertyGetter getter)
    {
        if (vec2Properties.ContainsKey(key))
        {
            Debug.LogWarning($"key '{key}' already exists");
            return;
        }
        vec2Properties.Add(key, new Property<Vector2>(setter, getter));
    }
    public void RegisterParam(string key, Property<Vector3>.PropertySetter setter, Property<Vector3>.PropertyGetter getter)
    {
        if (vec3Properties.ContainsKey(key))
        {
            Debug.LogWarning($"key '{key}' already exists");
            return;
        }
        vec3Properties.Add(key, new Property<Vector3>(setter, getter));
    }
    public void RegisterParam(string key, Property<string>.PropertySetter setter, Property<string>.PropertyGetter getter)
    {
        if (stringProperties.ContainsKey(key))
        {
            Debug.LogWarning($"key '{key}' already exists");
            return;
        }
        stringProperties.Add(key, new Property<string>(setter, getter));
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
