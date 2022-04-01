using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HakaiEndMessage : MonoBehaviour
{
    [HideInInspector]
    public bool endAnime=false;
    
    public Animator animator;
    public void IsOnEndAnime()
    {
        endAnime = true;

    }
}
