using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SetConversationPositionPlayableAsset : PlayableAsset
{
    public ExposedReference<GameObject> m_Player;
    public float m_Distance = 2;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        SetConversationPositionPlayableBehaviour behaviour = new SetConversationPositionPlayableBehaviour();
        behaviour.Player = m_Player.Resolve(graph.GetResolver());
        behaviour.Distance = m_Distance;

        return ScriptPlayable<SetConversationPositionPlayableBehaviour>.Create(graph, behaviour);
    }
}
