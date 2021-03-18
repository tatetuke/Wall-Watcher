using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warpGlow : MonoBehaviour
{
    private float t;
    // Start is called before the first frame update
    void Start()
    {
        t = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<SpriteGlow.SpriteGlowEffect>().GlowBrightness = 10.0f + 1.0f * Mathf.Sin(2 * Mathf.PI / 4.0f * t);
        t += Time.deltaTime;
    }
}
