using UnityEngine;
using Fungus;

class Test : MonoBehaviour
{
    public int money;
    public Flowchart flowchart;
    public FlowchartData flowchartData;

    public void EncodeFlowchart()
    {
        flowchartData = FlowchartData.Encode(flowchart);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //flowchart.SendFungusMessage("Start");
            EncodeFlowchart();
        }
    }

}

