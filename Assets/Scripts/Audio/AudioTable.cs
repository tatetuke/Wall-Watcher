using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// idとAudioClipの対応表を管理するクラス
/// </summary>
[CreateAssetMenu(fileName = "NewAudioTable", menuName = "ScriptableObject/AudioTable")]
public class AudioTable : ScriptableObject
{
    [HideInInspector, SerializeField] public List<AudioPiece> items = new List<AudioPiece>();

    public bool ContainsKey(string id)
    {
        foreach (var i in items)
            if (i.id == id) return true;
        return false;
        // return index.ContainsKey(id);
    }


    public AudioPiece GetPiece(string id)
    {
        foreach (var i in items)
        {
            //Debug.Log($"{i.id} , {i.reference}");
            if (i.id == id) return i;
        }
        return null;
    }

    /// <summary>
    /// リストに会話を追加する
    /// </summary>
    /// <param name="audioPiecePiece"></param>
    public void Add(AudioPiece audioPiece)
    {
        items.Add(audioPiece);
    }

    public void Set(AudioPiece originalAudioPiece, AudioPiece newAudioPiece)
    {
        for (var i = 0; i < items.Count; i++)
        {
            if (items[i].id == originalAudioPiece.id)
            {
                items[i] = newAudioPiece;
                break;
            }
        }
    }

    /// <summary>
    /// リストにあるidがidの要素を削除する
    /// </summary>
    /// <param name="id"></param>
    public void Delete(string id)
    {
        for (var i = 0; i < items.Count; i++)
        {
            if (items[i].id == id)
            {
                items.RemoveAt(i);
                break;
            }
        }
    }
}
