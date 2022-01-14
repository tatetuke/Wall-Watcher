﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]

public class PlayerPositionSet : MonoBehaviour
{
    private float autoMoveTime = 0.4f; 
    // Start is called before the first frame update
    void Start()
    {
        //int prev = AllMapSet.prevMap;
        //int crnt = AllMapSet.currentMap;
        float newx = AllMapSet.get_initial_position().Item1;
        float newy = AllMapSet.get_initial_position().Item2;
        this.transform.position = new Vector3(newx, newy, 0);


        if (AllMapSet.prevMap != AllMapSet.currentMap)   // この条件文は後で書き換える
          {
            StartCoroutine(autoMove());
          }

    }

    IEnumerator autoMove()
    {
        if (AllMapSet.autoWalkingDirection == 1)
        {
            this.GetComponent<Player>().ChangeState(Player.State.AUTOR);
        }
        else
        {
            this.GetComponent<Player>().ChangeState(Player.State.AUTOL);
        }
        
        yield return new WaitForSeconds(autoMoveTime);
        this.GetComponent<Player>().ChangeState(Player.State.WALKING);
    }
}
