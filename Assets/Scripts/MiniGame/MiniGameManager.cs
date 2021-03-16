using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    enum State
    {
        Mix,
        Paint,
        Result
    }
    State m_State = State.Mix;

    ScoreManager Score;
    [SerializeField] GameObject Mix;
    [SerializeField] GameObject Paint;
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] public Button FinishTaskButton;

    Text MixValue;
    Text SatisfactionValue;
    Text EarnRewardValue;

    int Diff;
    int MaxRGB;

    // Start is called before the first frame update
    void Start()
    {
        Score = new ScoreManager();
        Diff = 0;
        GameObject mixValueGameObject = GameObject.Find("ResultCanvas");
        MixValue = mixValueGameObject.transform.Find("MixValue").GetComponent<Text>();
        SatisfactionValue = mixValueGameObject.transform.Find("SatisfactionValue").GetComponent<Text>();
        EarnRewardValue = mixValueGameObject.transform.Find("EarnRewardValue").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (m_State == State.Mix)
        {
            Score.UpdateMix();
            MixValue.text = Score.GetMix().ToString();
            Mix.SetActive(false);
            Paint.SetActive(true);
            m_State = State.Paint;
        }
        else if (m_State == State.Paint)
        {
            Score.UpdateSatisfaction();
            SatisfactionValue.text = Score.GetSatisfaction().ToString();
            EarnRewardValue.text = Score.GetEarnReward().ToString();
            playableDirector.Play();
            FinishTaskButton.interactable = false;
        }
    }
}
