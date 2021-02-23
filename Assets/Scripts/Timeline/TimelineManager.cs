using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public PlayableDirector playableDirector;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Tキーが押され、再生中でないならば、再生する
        if (Input.GetKeyDown(KeyCode.T) && playableDirector.state != PlayState.Playing)
        {
            Debug.Log("タイムラインが再生されました");
            // タイムラインの再生
            playableDirector.Play();
        }
    }
}
