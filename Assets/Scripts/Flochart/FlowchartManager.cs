using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowchartManager : MonoBehaviour
{
    CinemachineBrain m_CinemachineBrain;
    CinemachineVirtualCamera m_CinemachineVirtualCamera;
    [SerializeField] GameObject m_Parent;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Osananazimi;
    // Start is called before the first frame update
    void Start()
    {
        GameObject camera = GameObject.Find("Main Camera");
        Debug.Log(camera);
        m_CinemachineBrain = camera.GetComponent<CinemachineBrain>();
        m_CinemachineVirtualCamera = camera.GetComponent<CinemachineVirtualCamera>();
    }

    public void SetActiveCinemachineBrain(bool isActive)
    {
        m_CinemachineBrain.enabled = isActive;
    }

    public void CreateParent()
    {
        Player.gameObject.transform.parent = m_Parent.gameObject.transform;
        Osananazimi.gameObject.transform.parent = m_Parent.gameObject.transform;
    }

    public void RelieveParent()
    {
        Player.gameObject.transform.parent = null;
        Osananazimi.gameObject.transform.parent = null;
    }

    public void SetCameraFollowObject(GameObject gameObject)
    {
        m_CinemachineVirtualCamera.Follow = gameObject.transform;
    }
}
