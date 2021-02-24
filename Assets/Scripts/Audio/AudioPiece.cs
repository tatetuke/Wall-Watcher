using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class AssetReferenceAudio : AssetReferenceT<AudioClip>
{
    public AssetReferenceAudio(string guid) : base(guid) { }
}

/// <summary>
/// 読み込む際に使うidと対応するAudioClipのデータ
/// </summary>
[System.Serializable]
public class AudioPiece
{
    [SerializeField] public string id;
    [SerializeField] public AssetReferenceAudio reference;
}
