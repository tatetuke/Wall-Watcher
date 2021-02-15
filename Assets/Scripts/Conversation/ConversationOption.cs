using UnityEngine;

namespace RPGM.Gameplay
{
    /// <summary>
    /// A choice in a conversation script.
    /// ‘I‘ğˆ
    /// </summary>
    [System.Serializable]
    public struct ConversationOption
    {
        public string text;//‚Í‚¢E‚¢‚¢‚¦‚È‚Ç
        public AudioClip audio;
        public string targetId;
        public bool enabled;

    }
}