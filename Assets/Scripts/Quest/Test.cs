using UnityEngine;
using Fungus;
using NaughtyAttributes;

class Test : MonoBehaviour
{
    public int money;
    public Flowchart flowchart;
    public FlowchartData flowchartData;

    [Button]
    public void EncodeFlowchart()
    {
        flowchartData = FlowchartData.Encode(flowchart);
    }

    [Button]
    public void DecodeFlowchart()
    {
        FlowchartData.Decode(flowchartData);
    }

    public int GetMoney()
    {
        EncodeFlowchart();
        return money;
    }

    public void SetMoney(int m)
    {
        money = m;
    }


}

