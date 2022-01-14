using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] Walls = GameObject.FindGameObjectsWithTag("WallColor");
        
        for (int i = 0; i < Walls.Length; i++)
        {
            Walls[i].GetComponent<SpriteRenderer>().color = new Color32(225, 250, 200, 255);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
