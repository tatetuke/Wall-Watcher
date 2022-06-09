using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class camera_relocate : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera vcamera;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float move_x = 0f;
    //[SerializeField]
    //private float move_y = 0f;

    [SerializeField]
    private float player_speed = 0.0f;

    private Vector3 camera_position_buf;
    private bool allow_right_relocate = false;
    private bool allow_left_relocate = false;
    private bool pressed_right_key = false;
    private bool pressed_left_key = false;
    private map.Position2D player_pos = map.Position2D.Invalid;


    void Start()
    {
        BoxCollider2D tmp = this.GetComponent<BoxCollider2D>();
        if (player.transform.position.x > this.transform.position.x + tmp.offset.x + tmp.size.x / 2)
        {
            player_pos = map.Position2D.Right;
        } else if (player.transform.position.x < this.transform.position.x + tmp.offset.x - tmp.size.x / 2)
        {
            player_pos = map.Position2D.Left;
        } else
        {
            Debug.LogError("player_position_error");
            if (UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.EditorApplication.isPaused = true;
            }
        }
    }

    void Update() {
        
        if (allow_right_relocate) {
            vcamera.transform.position +=  new Vector3(10, 0, 0) * Time.deltaTime;
            
            if (vcamera.transform.position.x >= camera_position_buf.x + move_x) {
                allow_right_relocate = false;
                player.GetComponent<Player>().stopAutoMove();
            }
        } else if (allow_left_relocate)
        {
            vcamera.transform.position -= new Vector3(10, 0, 0) * Time.deltaTime;

            if (vcamera.transform.position.x <= camera_position_buf.x - move_x) {
                allow_left_relocate = false;
                player.GetComponent<Player>().stopAutoMove();
            }
        }
    }
    
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            camera_position_buf = vcamera.transform.position;   // 移動前のカメラの位置を保持
            if (player_pos == map.Position2D.Left)
            {
                allow_right_relocate = true;
                allow_left_relocate = false;
                Debug.Log(player_speed);
                player.GetComponent<Player>().autoMove(player_speed, map.Direction2D.Right);  // プレイヤー自動移動
                player_pos = map.Position2D.Right;
            } else if (player_pos == map.Position2D.Right)
            {
                allow_left_relocate = true;
                allow_right_relocate = false;
                player.GetComponent<Player>().autoMove(player_speed, map.Direction2D.Left);
                player_pos = map.Position2D.Left;
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            //player.GetComponent<Player>().ChangeState(Player.State.WALKING);
            //allow_right_relocate = false;
        }
    }
}
