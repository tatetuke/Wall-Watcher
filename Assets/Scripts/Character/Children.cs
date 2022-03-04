using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Children : MonoBehaviour
{
    [SerializeField] Text m_Text;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Transform target = null;
    [SerializeField] GameObject view = null;

    void Awake()
    {
        Vector3 pos = target.position;
        pos.y += 1.5f;
        rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPosition()
    {
        rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, view.transform.position);
    }

    public void SetText(string s)
    {
        Vector3 pos = target.position;
        pos.y += 1.5f;
        rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
        m_Text.text = s;
    }

    public void ClearText()
    {
        m_Text.text = "";
    }
}
