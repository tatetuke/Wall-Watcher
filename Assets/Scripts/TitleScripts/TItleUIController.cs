using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TItleUIController : MonoBehaviour
{
    [SerializeField]
    Animator PushAnyKeyAnimator;
    [SerializeField]
    Animator TitleLogoAnimator;
    
    void Update()
    {
        if (Input.anyKey)
        {
            Debug.Log("何らかのキーが押されました");
            PushAnyKeyAnimator.SetBool("IsPushAnyKey", true);
            TitleLogoAnimator.SetBool("IsPushAnyKey", true);
        }
    }
}
