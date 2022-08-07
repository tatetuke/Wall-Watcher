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
        SetGlowLine(this.gameObject, Color.white);
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SetGlowLine(this.gameObject, Color.yellow);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SetGlowLine(this.gameObject, Color.white);
    }


    void SetGlowLine(GameObject gameObject, Color color)
    {
        if (gameObject == null) return;
        Material m_material = gameObject.GetComponent<Renderer>().material;
        m_material.SetColor("Color_7C7012AB", color);
    }
}
