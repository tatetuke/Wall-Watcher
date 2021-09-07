using System.Collections;
using UnityEngine;
using Fungus;

/// <summary>
/// Flowchart内の変数をセーブ・ロードする
/// セーブはFlowchartDataContainerを通してDataBankへ、
/// ロードはFlowchartDataContainerを通してDataBankから行う。
/// ロード時、接頭語が _ の場合のみGamePropertyManagerから読み込む
/// </summary>
public class FlowchartSaveLoader:MonoBehaviour
{
    //"Flowchart内の変数の扱われ方について：
    //先頭の文字が _ (アンダーバー)で始まるならグローバル変数としてGamePropertyManagerから読みこまれる

    //それ以外はクエスト毎のローカル変数として読み書きされる
    //クエスト名が共通ならFlowchartDataも共通のものとしている

    public Flowchart flowchart;

    [NaughtyAttributes.Button]
    /// public void SaveFlowchartVariables(Flowchart flowchart)
    /// <summary>Flowchart内の変数をDataBankへセーブする</summary>
    public void SaveFlowchartVariables()
    {
        string key = GetKey(flowchart);
        FlowchartData flowchartData = FlowchartData.Encode(flowchart);

        FlowchartDataContainer.Instance.Store(key, flowchartData);
    }

    [NaughtyAttributes.Button]
    //public void LoadFlowchartVariables(Flowchart flowchart)
    /// <summary>Flowchart内の変数をDataBankからロードする 接頭語が _ の場合のみGamePropertyManagerからロードする</summary>
    public void LoadFlowchartVariables()
    {
        string key = GetKey(flowchart);
        FlowchartData flowchartData = FlowchartDataContainer.Instance.Get(key);
        
        FlowchartData.Decode(flowchart, flowchartData);

        //接頭語が _ のときだけGamePropertyManagerから読む
        LoadFromGamePropertyManager(flowchart, flowchartData);

    }


    public string GetKey(Flowchart flowchart)
    {
        return flowchart.gameObject.name;
    }

    public void LoadFromGamePropertyManager(Flowchart flowchart, FlowchartData flowchartData)
    {
        //接頭語が _ のときだけGamePropertyManagerから読む
        for (int i = 0; i < flowchartData.BoolVars.Count; i++)
        {
            var boolVar = flowchartData.BoolVars[i];
            if (boolVar.Key[0] == '_')
            {
                bool var = GamePropertyManager.Instance.GetProperty<bool>(boolVar.Key);
                flowchart.SetBooleanVariable(boolVar.Key, var);
            }
        }
        for (int i = 0; i < flowchartData.IntVars.Count; i++)
        {
            var intVar = flowchartData.IntVars[i];
            if (intVar.Key[0] == '_')
            {
                int var = GamePropertyManager.Instance.GetProperty<int>(intVar.Key);
                flowchart.SetIntegerVariable(intVar.Key, var);
            }
        }
        for (int i = 0; i < flowchartData.FloatVars.Count; i++)
        {
            var floatVar = flowchartData.FloatVars[i];
            if (floatVar.Key[0] == '_')
            {
                float var = GamePropertyManager.Instance.GetProperty<float>(floatVar.Key);
                flowchart.SetFloatVariable(floatVar.Key, var);
            }
        }
        for (int i = 0; i < flowchartData.StringVars.Count; i++)
        {
            var stringVar = flowchartData.StringVars[i];
            if (stringVar.Key[0] == '_')
            {
                string var = GamePropertyManager.Instance.GetProperty<string>(stringVar.Key);
                flowchart.SetStringVariable(stringVar.Key, var);
            }
        }

        // flowchartDataになかった変数もロードするためもう一回
        FlowchartData flowchartData2 = FlowchartData.Encode(flowchart);
        //接頭語が _ のときだけGamePropertyManagerから読む
        for (int i = 0; i < flowchartData2.BoolVars.Count; i++)
        {
            var boolVar = flowchartData2.BoolVars[i];
            if (boolVar.Key[0] == '_')
            {
                bool var = GamePropertyManager.Instance.GetProperty<bool>(boolVar.Key);
                flowchart.SetBooleanVariable(boolVar.Key, var);
            }
        }
        for (int i = 0; i < flowchartData2.IntVars.Count; i++)
        {
            var intVar = flowchartData2.IntVars[i];
            if (intVar.Key[0] == '_')
            {
                int var = GamePropertyManager.Instance.GetProperty<int>(intVar.Key);
                flowchart.SetIntegerVariable(intVar.Key, var);
            }
        }
        for (int i = 0; i < flowchartData2.FloatVars.Count; i++)
        {
            var floatVar = flowchartData2.FloatVars[i];
            if (floatVar.Key[0] == '_')
            {
                float var = GamePropertyManager.Instance.GetProperty<float>(floatVar.Key);
                flowchart.SetFloatVariable(floatVar.Key, var);
            }
        }
        for (int i = 0; i < flowchartData2.StringVars.Count; i++)
        {
            var stringVar = flowchartData2.StringVars[i];
            if (stringVar.Key[0] == '_')
            {
                string var = GamePropertyManager.Instance.GetProperty<string>(stringVar.Key);
                flowchart.SetStringVariable(stringVar.Key, var);
            }
        }

    }

}
