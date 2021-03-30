using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionManager : MonoBehaviour
{
    public GameObject PointMarker;
    //GameObject PointMarker = (GameObject)Resources.Load("Prefabs/PointMarker");
    float[] X = new float[] { -65, -35, -25, -15, -5, 5, 15, 55, 65 };

    // Start is called before the first frame update
    void Start()
    {
        if (PointMarker == null)
        {
            Debug.Log("null");
        }
        else
        {
            Debug.Log("nullじゃない");
        }
        for (int i = 0; i < X.Length; i++)
        {
            if (Random.value <= 0.2f)
                Instantiate(PointMarker, new Vector3(X[i], 0.0f, 0.0f), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
