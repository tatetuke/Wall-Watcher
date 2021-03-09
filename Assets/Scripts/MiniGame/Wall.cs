using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public enum WallState
    {
        CRACKED,
        DRY,
        PAINTED,
    }


    public Sprite[] Sprites;


    private WallState ThisState;

    private int ClickNum;
    public byte ColorNum;



    // Start is called before the first frame update
    void Start()
    {
        if (Random.value <= 0.1f) ThisState = WallState.CRACKED;
        else ThisState = WallState.DRY;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = Sprites[(int)ThisState];

        ClickNum = 0;
        ColorNum = 255;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public WallState GetState()
    {
        return ThisState;
    }

    public void ChangeState(WallState wallState)
    {
        ThisState = wallState;
    }


    public void ChangeSprite(WallState wallState)
    {
        ChangeState(wallState);
        this.gameObject.GetComponent<SpriteRenderer>().sprite = Sprites[(int)wallState];
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
