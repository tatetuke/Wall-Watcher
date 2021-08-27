using System.Collections;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(Flowchart))]
/// <summary>
/// 会話開始時にGamePropertyManagerからフラグなどを読込
/// 会話開始時にGamePropertyManagerへ　フラグなどを書込
/// </summary>
public class FlowchartSaveLoader : MonoBehaviour
{
    [ReadOnly, SerializeField,
        NaughtyAttributes.InfoBox(
        "Flowchart内の変数の扱われ方について：\n先頭の文字が_(アンダーバー)で始まるならグローバル変数として読み書きされる。\nそれ以外はクエスト毎のローカル変数として読み書きされる。"),
        ] 
    private Flowchart m_Flowchart;
    

    private void Start()
    {
        m_Flowchart = GetComponent<Flowchart>();
    }

    public void SaveFlags()
    {

    }

    public void LoadFlags()
    {

    }


}
