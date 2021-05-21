using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    public enum State
    {
        NORMAL, // 通常状態
        BROKEN, // 壊された状態
        PAINTED, // 塗られた状態
    }
    private State m_State = default;

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
            if(m_State == State.NORMAL) SetGlowLine(Color.cyan);
            else if (m_State == State.BROKEN) SetGlowLine(Color.blue);
        }
    }
    public void breakwall()
    {
        m_State = State.BROKEN;
    }

    public void paintwall()
    {
        m_State = State.PAINTED;
    }

    void SetGlowLine(Color color)
    {
        Material material = this.GetComponent<Renderer>().material;
        material.color = color;
        //material.SetColor("Color_7C7012AB", color);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (IsInCollider())
            {
                //FadeManager.Instance.LoadLevel("Paint", 1f);
                breakwall();

                Debug.Log("MiniGamePaintシーンに遷移!");
            }
        }
    }

    private bool IsInCollider()
    {
        Material material = this.GetComponent<Renderer>().material;
        return material.color == Color.yellow;
        //return material.GetColor("Color_7C7012AB") == Color.yellow;
    }
}
