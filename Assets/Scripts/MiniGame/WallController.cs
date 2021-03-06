using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    private int ClickNum;

    // Start is called before the first frame update
    void Start()
    {
        ClickNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetClickNum() 
    { 
        return ClickNum; 
    }

    public void CountUp()
    {
        ClickNum++;
    }
}
