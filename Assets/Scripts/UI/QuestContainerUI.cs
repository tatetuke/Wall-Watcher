using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class QuestContainerUI : MonoBehaviour
{
    [SerializeField] Image questStateIcon;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] Button m_button;

    public void SetOnClicked(UnityAction action)
    {
        m_button.onClick.AddListener(action);
    }

}
