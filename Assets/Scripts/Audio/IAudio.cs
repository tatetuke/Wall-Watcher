using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 再生・ストップなどを提供する
/// </summary>
interface IAudio
{
    void Stop();

    bool IsPlaying();
}
