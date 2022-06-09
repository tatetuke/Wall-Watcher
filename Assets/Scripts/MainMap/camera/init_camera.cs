using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class init_camera : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera vcamera;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private bool follow = true;

    // Start is called before the first frame update
    void Start()
    {
        if (follow) vcamera.Follow = player.transform;
        else vcamera.Follow = null;
    }
}
