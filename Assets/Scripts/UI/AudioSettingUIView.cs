using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自作シーンの音量調節UI用
/// </summary>
public class AudioSettingUIView : UIView
{
    [SerializeField] private GameObject audioSettingUIObject;

    // Start is called before the first frame update
    void Start()
    {
        audioSettingUIObject = gameObject;

        OnViewShow.AddListener(() =>
        {
            audioSettingUIObject.SetActive(true);
        });
        OnViewHide.AddListener(() =>
        {
            audioSettingUIObject.SetActive(false);
        });

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            OnViewShow.Invoke();
        }

    }

    public void CloseUI()
    {
        OnViewHide.Invoke();
    }

}
