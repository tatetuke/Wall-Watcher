using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class MiniGameManager : MonoBehaviour
{
    enum State
    {
        Mix,
        Paint,
        Result
    }
    State m_State = State.Mix;

    [SerializeField] GameObject Mix;
    [SerializeField] GameObject Paint;
    [SerializeField] private PlayableDirector playableDirector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        Debug.Log("ボタンが押されました");
        if (m_State == State.Mix)
        {
            Mix.SetActive(false);
            Paint.SetActive(true);
            m_State = State.Paint;
        }
        else if (m_State == State.Paint)
        {
            playableDirector.Play();

        }
        //GameObject[] g = GameObject.FindGameObjectsWithTag("NPC");
        //foreach(var a in g)
        //{
        //    a.GetComponent<SpriteRenderer>().color = Color.gray;
        //}
        ////g.GetComponent<SpriteRenderer>().color = Color.gray;
    }
}
