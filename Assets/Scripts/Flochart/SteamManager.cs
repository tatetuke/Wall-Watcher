using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamManager : MonoBehaviour
{
    [SerializeField]
    Flowchart flowchart;
    [SerializeField]
    GameObject kitchen;
    // Start is called before the first frame update
    void Start()
    {
        SetGlowLine(kitchen, Color.white);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            flowchart.SendFungusMessage("StartSteam");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SetGlowLine(kitchen, Color.yellow);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SetGlowLine(kitchen, Color.white);
    }


    void SetGlowLine(GameObject gameObject, Color color)
    {
        if (gameObject == null) return;
        Material m_material = kitchen.GetComponent<Renderer>().material;
        m_material.SetColor("Color_7C7012AB", color);
    }
}
