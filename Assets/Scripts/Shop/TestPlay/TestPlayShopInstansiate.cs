﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayShopInstansiate : MonoBehaviour
{
  
    public void OnClickButton()
    {
        GameObject prefab = (GameObject)Resources.Load("Shop/Shop");
        GameObject instance = (GameObject)Instantiate(prefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
    }

}
