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
    const string floatListHeader = "property.floatList";
    const string boolHeader = "property.bool";
    const string stringHeader = "property.string";
    [Header("Property")]
    [SerializeField] string directory = "Data";
    [SerializeField] string filename = "propertydata.csv";
    Dictionary<string, List<string>> m_properties = new Dictionary<string, List<string>>();
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
            List<string> value = new List<string>();
            for (int i = 2; i < obj.Count; i++) value.Add(obj[i]);
            m_propertyTypes.Add(key, typeID);
            m_properties.Add(key, value);
        }
    }

    public void SaveProperty()
    {
        Debug.Log("Save properties", gameObject);
        var list = new List<List<string>>();

        CSVReader.Write(directory, filename, list);
    }

    public List<string> Get(string key)
    {
        return m_properties[key];
    }
    public int GetInt(string key,int default_=0)
    {
        if (!m_propertyTypes.ContainsKey(key)||m_propertyTypes[key] != intHeader)
        {
            return default_;
        }
        return int.Parse(m_properties[key][0]);
    }
    public float GetFloat(string key,float default_=0f)
    {
        if (!m_propertyTypes.ContainsKey(key) || m_propertyTypes[key] != floatHeader)
        {
            return default_;
        }
        return float.Parse(m_properties[key][0]);
    }
    public bool GetBool(string key,bool default_=false)
    {
        if (!m_propertyTypes.ContainsKey(key) || m_propertyTypes[key] != boolHeader)
        {
            return default_;
        }
        return bool.Parse(m_properties[key][0]);
    }
    public string GetString(string key,string default_="")
    {
        if (!m_propertyTypes.ContainsKey(key) || m_propertyTypes[key] != stringHeader)
        {
            return default_;
        }
        return m_properties[key][0];
    }
    public Vector2 GetVec2(string key,Vector2 default_)
    {
        if (!m_propertyTypes.ContainsKey(key) || m_propertyTypes[key] != floatListHeader)
        {
            return default_;
        }
        return GetVec2(key);
    }
    public Vector3 GetVec3(string key, Vector3 default_)
    {
        if (!m_propertyTypes.ContainsKey(key) || m_propertyTypes[key] != floatListHeader)
        {
            return default_;
        }
        return GetVec3(key);
    }
    public Vector2 GetVec2(string key)
    {
        if (!m_propertyTypes.ContainsKey(key) || m_propertyTypes[key] != floatListHeader)
        {
            return Vector2.zero;
        }
        float x = float.Parse(m_properties[key][0]);
        float y = float.Parse(m_properties[key][1]);
        return new Vector2(x, y);
    }
    public Vector3 GetVec3(string key)
    {
        if (!m_propertyTypes.ContainsKey(key) || m_propertyTypes[key] != floatListHeader)
        {
            return Vector3.zero;
        }
        float x = float.Parse(m_properties[key][0]);
        float y = float.Parse(m_properties[key][1]);
        float z = float.Parse(m_properties[key][2]);
        return new Vector3(x, y, z);
    }
    public void SetInt(string key, int value)
    {
        if (m_propertyTypes[key] != intHeader)
        {
            return;
        }
        m_properties[key][0] = value.ToString();
    }
    public void SetFloat(string key, float value)
    {
        if (m_propertyTypes[key] != floatHeader)
        {
            return;
        }
        m_properties[key][0] = value.ToString();
    }
    public void SetBool(string key, bool value)
    {
        if (m_propertyTypes[key] != boolHeader)
        {
            return;
        }
        m_properties[key][0] = value.ToString();
    }
    public void SetString(string key, string value)
    {
        if (m_propertyTypes[key] != stringHeader)
        {
            return;
        }
        m_properties[key][0] = value;
    }
    public void SetVec2(string key, Vector2 value)
    {
        SetFloatList(key, value.x, value.y);
    }
    public void SetVec3(string key, Vector3 value)
    {
        SetFloatList(key, value.x, value.y, value.z);
    }

    public void SetFloatList(string key, IEnumerable<float> values) {
        if (m_propertyTypes[key] != floatListHeader)
        {
            return;
        }
        var list = m_properties[key];
        int count = 0;
        foreach(var i in values)
        {
            if (count < list.Count)
            {
                list[count] = i.ToString();
                count++;
            }
            else
            {
                list.Add(i.ToString());
            }
        }
    }

    public void SetFloatList(string key, params float[] args)
    {
        if (m_propertyTypes[key] != floatListHeader)
        {
            return;
        }
        var list = m_properties[key];
        int count = 0;
        foreach (var i in args)
        {
            if (count < list.Count)
            {
                list[count] = i.ToString();
                count++;
            }
            else
            {
                list.Add(i.ToString());
            }
        }
    }


}
