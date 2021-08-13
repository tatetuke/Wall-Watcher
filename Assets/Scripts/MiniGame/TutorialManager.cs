using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] Animator Anim;
    [SerializeField] Image TutorialImage;
    [SerializeField] Text TutorialText;
    [SerializeField] Text Page;
    [SerializeField] Sprite[] Images;
    [SerializeField] string[] Explanations;
    private int NowPage = 0;
    private int TutorialSize;

    // Start is called before the first frame update
    void Start()
    {
        TutorialSize = Images.Length;
        TutorialImage.sprite = Images[0];
        TutorialText.text = Explanations[0];
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClickBack()
    {
        if (NowPage != 0)
        {
            NowPage--;
            UpdatePage(NowPage + 1);
        }
        UpdateTutorial();
    }

    public void OnClickNext()
    {
        if (NowPage < TutorialSize - 1)
        {
            NowPage++;
            UpdatePage(NowPage + 1);
        }
        UpdateTutorial();
    }

    void UpdateTutorial()
    {
        TutorialImage.sprite = Images[NowPage];
        TutorialText.text = Explanations[NowPage];
    }

    public void OnClickTutorial()
    {
        bool b = Anim.GetBool("IsSmall");
        Anim.SetBool("IsSmall", !b);
    }

    void UpdatePage(int num)
    {
        Page.text = num.ToString() + "/" + TutorialSize.ToString();
    }
}