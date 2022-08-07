using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SwitchManager : MonoBehaviour
{
    [SerializeField]
    Light2D Light;

    int time_frame = 90;

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
            if (Light.intensity == 0)
            {
                StartCoroutine("TurnOn");
            }
            else if(Light.intensity == 1)
            {
                StartCoroutine("TurnOff");
            }
        }
    }

    IEnumerator TurnOn()
    {
        for (int i = 0; i < time_frame + 1; i++)
        {
            Light.intensity = Mathf.Clamp(Light.intensity + 1.0f / time_frame, 0, 1);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator TurnOff()
    {
        for (int i = 0; i < time_frame + 1; i++)
        {
            Light.intensity = Mathf.Clamp(Light.intensity - 1.0f / time_frame, 0, 1);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
