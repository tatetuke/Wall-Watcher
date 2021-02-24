using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    public Button backButton;
    public UnityEvent OnViewShow = new UnityEvent();
    public UnityEvent OnViewHide = new UnityEvent();
}
