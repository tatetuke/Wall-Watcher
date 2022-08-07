using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(Vector3 v,string s)
    {
        Sign.pos = v;
        Sign.scene_name = s;
    }

    public void Show()
    {
        if (SceneManager.GetActiveScene().name == Sign.scene_name)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            this.gameObject.transform.position = Sign.pos;
        }
    }

    public void Clear()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
