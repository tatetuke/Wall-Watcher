using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector playableDirector;
    [SerializeField]
    private Player player;
    private bool IsStarted;
    private bool IsEnd;

    // Start is called before the first frame update
    void Start()
    {
        IsStarted = false;
        IsEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Tキーが押され、再生中でないならば、再生する
        if (Input.GetKeyDown(KeyCode.T) && playableDirector.state != PlayState.Playing)
        {
            IsStarted = true;
            Debug.Log("タイムラインが再生されました");
            player.ChangeState(Player.State.FREEZE);
            // タイムラインの再生
            playableDirector.Play();
        }

        if(IsStarted && playableDirector.state != PlayState.Playing)
        {
            Debug.Log("タイムライン終了");
            player.ChangeState(Player.State.IDEL);
        }
    }
}
