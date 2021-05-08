using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetGlowLine(Color.yellow);
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
        //material.color = color;
        material.SetColor("Color_7C7012AB", color);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetGlowLine(Color.cyan);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (IsInCollider())
            {
                FadeManager.Instance.LoadLevel("Paint", 1f);
                Debug.Log("MiniGamePaintシーンに遷移!");
            }
        }
    }

    private bool IsInCollider()
    {
        Material material = this.GetComponent<Renderer>().material;
        //return material.color == Color.yellow;
        return material.GetColor("Color_7C7012AB") == Color.yellow;
    }
}
