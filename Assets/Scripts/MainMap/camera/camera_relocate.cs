using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class camera_relocate : MonoBehaviour
{
    public GameObject player;
    //public GameObject vcamera;
    public CinemachineVirtualCamera vcamera;
    private bool allow_right_relocate;
    private bool pressed_right_key;
    void Start()
    {
        allow_right_relocate = false;
        vcamera.Follow = null;
    }

    void Update() {
        
        if (allow_right_relocate) {
            vcamera.transform.position +=  new Vector3(10, 0, 0) * Time.deltaTime;
            
            if (vcamera.transform.position.x >= 12) {
                allow_right_relocate = false;
                player.GetComponent<Player>().stopAutoMove();
            }
        }
    }
    
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            vcamera.Follow = null;
            allow_right_relocate = true;
            player.GetComponent<Player>().autoMove(3f, 2f, map.Direction2D.Right);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            //player.GetComponent<Player>().ChangeState(Player.State.WALKING);
            //allow_right_relocate = false;
        }
    }
}
