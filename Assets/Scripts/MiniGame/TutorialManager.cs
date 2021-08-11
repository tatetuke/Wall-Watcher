using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Image TutorialImage;
    [SerializeField] Text TutorialText;
    [SerializeField] Sprite[] Images;
    [SerializeField] string[] Explanations;
    private int Now = 0;
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
        if (Now != 0) Now--;
        UpdateTutorial();
    }

    public void OnClickNext()
    {
        if (Now < TutorialSize - 1) Now++;
        UpdateTutorial();
    }

    void UpdateTutorial()
    {
        TutorialImage.sprite = Images[Now];
        TutorialText.text = Explanations[Now];
    }

    public void OnClickTutorial()
    {
        Debug.Log("a");
        bool b = anim.GetBool("IsSmall");
        anim.SetBool("IsSmall", !b);
    }
}
