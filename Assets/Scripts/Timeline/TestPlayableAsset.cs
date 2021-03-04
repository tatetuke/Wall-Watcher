using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// アセット部分の役割を担う
/// </summary>

[System.Serializable]
public class TestPlayableAsset : PlayableAsset
{
    //public ExposedReference<GameObject> mPlayer;

    
    public ExposedReference<GameObject> m_Player;




    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        TestPlayableBehaviour behaviour = new TestPlayableBehaviour();
        behaviour.Player = m_Player.Resolve(graph.GetResolver());

        return ScriptPlayable<TestPlayableBehaviour>.Create(graph, behaviour);

        //return Playable.Create(graph);
        //return Playable.Create(graph, behaviour);
    }
}
