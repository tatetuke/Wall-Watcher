using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// ゲーム内のいろいろな変数を一つのファイルに保存・読み書きするためのクラス
/// </summary>
public class PropertyLoader : SingletonMonoBehaviour<PropertyLoader>
{
    //変数をファイルに保存するとき、その変数がどの型なのかを記述するヘッダ
    const string intHeader = "property.int";
    const string floatHeader = "property.float";
    const string boolHeader = "property.bool";
    const string vec2Header = "property.vec2";
    const string vec3Header = "property.vec3";
    const string stringHeader = "property.string";
    [Header("Property")]
    [SerializeField] string directory = "Data";
    [SerializeField] string filename = "propertydata.csv";
    Dictionary<string, string> m_properties = new Dictionary<string, string>();
    Dictionary<string, string> m_propertyTypes = new Dictionary<string, string>();
    /// <summary>
    /// RegisterParamした後に、そのパラメータをcsvファイルからロードした値で上書きする
    /// </summary>
    public void LoadProperty()
    {
        Debug.Log("Load properties", gameObject);
        var list = CSVReader.Read(directory, filename);
        if (list == null)
        {
            Debug.LogWarning("Property file not found", gameObject);
            return;
        }
        foreach (var obj in list)
        {
            string typeID = obj[0];
            string key = obj[1];
            StringBuilder sb = new StringBuilder(obj[2]);
            for (int i = 3; i < obj.Count; i++) sb.Append(obj[i]);
            m_propertyTypes.Add(key, typeID);
            m_properties.Add(key, sb.ToString());
        }
    }

    public void SaveProperty()
    {
        Debug.Log("Save properties", gameObject);
        var list = new List<List<string>>();

        CSVReader.Write(directory, filename, list);
    }

    public string Get(string key)
    {
        return m_properties[key];
    }
    public int GetInt(string key,int default_=0)
    {
        if (!m_propertyTypes.ContainsKey(key)||m_propertyTypes[key] != intHeader)
        {
            return default_;
        }
        return int.Parse(m_properties[key]);
    }
    public float GetFloat(string key,float default_=0f)
    {
        if (!m_propertyTypes.ContainsKey(key) || m_propertyTypes[key] != floatHeader)
        {
            return default_;
        }
        return float.Parse(m_properties[key]);
    }
    public bool GetBool(string key,bool default_=false)
    {
        if (!m_propertyTypes.ContainsKey(key) || m_propertyTypes[key] != boolHeader)
        {
            return default_;
        }
        return bool.Parse(m_properties[key]);
    }
    public string GetString(string key,string default_="")
    {
        if (!m_propertyTypes.ContainsKey(key) || m_propertyTypes[key] != stringHeader)
        {
            return default_;
        }
        return m_properties[key];
    }
    public Vector2 GetVec2(string key,Vector2 default_)
    {
        if (!m_propertyTypes.ContainsKey(key) || m_propertyTypes[key] != vec2Header)
        {
            return default_;
        }
        return GetVec2(key);
    }
    public Vector3 GetVec3(string key, Vector3 default_)
    {
        if (!m_propertyTypes.ContainsKey(key) || m_propertyTypes[key] != vec3Header)
        {
            return default_;
        }
        return GetVec3(key);
    }
    public Vector2 GetVec2(string key)
    {
        if (!m_propertyTypes.ContainsKey(key) || m_propertyTypes[key] != vec2Header)
        {
            return Vector2.zero;
        }
        var lis = m_properties[key].Split(',');
        float x = float.Parse(lis[0]);
        float y = float.Parse(lis[1]);
        return new Vector2(x, y);
    }
    public Vector3 GetVec3(string key)
    {
        if (!m_propertyTypes.ContainsKey(key) || m_propertyTypes[key] != vec3Header)
        {
            return Vector3.zero;
        }
        var lis = m_properties[key].Split(',');
        float x = float.Parse(lis[0]);
        float y = float.Parse(lis[1]);
        float z = float.Parse(lis[2]);
        return new Vector3(x, y, z);
    }
    public void SetInt(string key, int value)
    {
        if (m_propertyTypes[key] != intHeader)
        {
            return;
        }
        m_properties[key] = value.ToString();
    }
    public void SetFloat(string key, float value)
    {
        if (m_propertyTypes[key] != floatHeader)
        {
            return;
        }
        m_properties[key] = value.ToString();
    }
    public void SetBool(string key, bool value)
    {
        if (m_propertyTypes[key] != boolHeader)
        {
            return;
        }
        m_properties[key] = value.ToString();
    }
    public void SetString(string key, string value)
    {
        if (m_propertyTypes[key] != stringHeader)
        {
            return;
        }
        m_properties[key] = value;
    }
    public void SetVec2(string key, Vector2 value)
    {
        if (m_propertyTypes[key] != vec2Header)
        {
            return;
        }
        StringBuilder sb = new StringBuilder(value.x.ToString());
        sb.Append(value.y.ToString());
        m_properties[key] = sb.ToString();
    }
    public void SetVec3(string key, Vector3 value)
    {
        if (m_propertyTypes[key] != vec3Header)
        {
            return;
        }
        StringBuilder sb = new StringBuilder(value.x.ToString());
        sb.Append(value.y.ToString());
        sb.Append(value.z.ToString());
        m_properties[key] = sb.ToString();
    }
}
