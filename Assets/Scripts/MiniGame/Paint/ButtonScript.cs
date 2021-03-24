using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public GameObject Description0;
    public GameObject Description1;

    public void OnClick()
    {
        if (Description0.activeSelf)
        {
            Description0.SetActive(false);
            Description1.SetActive(false);
        }
        else
        {
            Description0.SetActive(true);
            Description1.SetActive(true);
        }
    }
}
