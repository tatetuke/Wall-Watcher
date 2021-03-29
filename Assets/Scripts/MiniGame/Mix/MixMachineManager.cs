using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixMachineManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetGlowLine(Color.red);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetGlowLine(Color.cyan);
        }
    }

    void SetGlowLine(Color color)
    {
        Material material = this.GetComponent<Renderer>().material;
        material.SetColor("Color_7C7012AB", color);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
