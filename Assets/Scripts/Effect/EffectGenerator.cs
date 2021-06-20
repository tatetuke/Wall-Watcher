using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectGenerator : SingletonMonoBehaviour<EffectGenerator>
{
    [System.Serializable]
    public class EffectData
    {
        public string name;
        public GameObject prefab;
    }
    [SerializeField]
    Transform effectParent;
    public List<EffectData> data = new List<EffectData>();

    public void Generate(string name)
    {
        foreach(var i in data)
        {
            if (i.name == name)
            {
                Instantiate(i.prefab, effectParent);
                return;
            }
        }
    }

}
