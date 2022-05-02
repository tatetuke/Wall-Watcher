using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KamishibaiImage : MonoBehaviour
{
    private Image m_Image;
    // Start is called before the first frame update
    void Start()
    {
        m_Image = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAlpha(float n)
    {
        Color color = m_Image.color;
        color.a = n;
        m_Image.color = color;
    }
}
