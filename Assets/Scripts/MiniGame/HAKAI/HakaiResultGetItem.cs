using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HakaiResultGetItem : MonoBehaviour
{
    public GameObject itemIcon;
    public GameObject itemDiscription;
    public GameObject itemInInventoryNum;
    public GameObject itemGetNum;
    public GameObject itemName;
    public Animator animator;


    public bool endFadeInAnime=false;

    public void IsOnEndFadeInAnime()
    {
        endFadeInAnime = true;
    }

}
