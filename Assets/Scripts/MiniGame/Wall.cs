using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    const int CRACKED = 0;
    const int NORMAL = 1;
    const int PAINTED = 2;

    public Sprite[] Sprites;


    private int State;

    private int ClickNum;
    public byte ColorNum;



    // Start is called before the first frame update
    void Start()
    {
        if (Random.value <= 0.1f) State = CRACKED;
        else State = NORMAL;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = Sprites[State];

        ClickNum = 0;
        ColorNum = 255;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetState()
    {
        return State;
    }



    public void ChangeSprite(int num)
    {
        if (State == CRACKED)
        {
            if (num > 0) return;
            State = NORMAL;
        }
        else if (State == NORMAL)
        {
            if (num < 0) return;
            State = PAINTED;
        }
        else if (State == PAINTED)
        {
            return;
        }

        this.gameObject.GetComponent<SpriteRenderer>().sprite = Sprites[State];
    }

    public int GetClickNum()
    {
        return ClickNum;
    }

    public void CountUp()
    {
        ClickNum++;
    }

    public void Change()
    {
        this.GetComponent<SpriteRenderer>().color = Color.gray;
    }
}
