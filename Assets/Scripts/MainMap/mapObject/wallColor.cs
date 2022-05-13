using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallColor : MonoBehaviour
{
    public byte R = 0;
    public byte G = 0;
    public byte B = 0;
    public byte A = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] Walls = GameObject.FindGameObjectsWithTag("WallColor");
        
        for (int i = 0; i < Walls.Length; i++)
        {
            Walls[i].GetComponent<SpriteRenderer>().color = new Color32(R, G, B, A);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
