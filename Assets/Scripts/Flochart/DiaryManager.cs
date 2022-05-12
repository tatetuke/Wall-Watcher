using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryManager : MonoBehaviour
{
    [SerializeField]
    Flowchart flowchart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            flowchart.SendFungusMessage("StartDiary");
        }
        Debug.Log("すり抜けている");
    }
}
