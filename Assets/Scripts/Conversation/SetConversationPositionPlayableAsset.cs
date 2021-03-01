using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SetConversationPositionPlayableAsset : PlayableAsset
{
    public ExposedReference<GameObject> m_Player;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        SetConversationPositionPlayableBehaviour behaviour = new SetConversationPositionPlayableBehaviour();
        behaviour.Player = m_Player.Resolve(graph.GetResolver());

        return ScriptPlayable<SetConversationPositionPlayableBehaviour>.Create(graph, behaviour);
    }
}
