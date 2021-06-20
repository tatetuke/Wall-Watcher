using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionManager : MonoBehaviour
{
    public GameObject PointMarker;
    private int num_of_pointMarker = 0;  // markerの数
    float[] marker_positison_x = new float[] { -65, -35, -25, -15, -5, 5, 15, 55, 65 };
    static private bool[] marker_flag = new bool[] { false, false, false, false, false, false, false, false, false }; // 生成された marker の管理フラグのリスト
    static GameObject bluewall; // 青色壁面のゲームオブジェクト
    public static GameObject selected_marker;  // 選択されたマーカーオブジェクト

    public static void breakMarker()
    {
        selected_marker.GetComponent<WallManager>().breakwall();
    }

    // Start is called before the first frame update
    void Start()
    {
        bluewall = GameObject.Find("ww_3_hekimen_5");
        bluewall.SetActive(false);  // 青背景無効

        if (PointMarker == null)
        {
            Debug.Log("null");
        }
        else
        {
            Debug.Log("nullじゃない");
        }
        for (int i = 0; i < marker_positison_x.Length; i++)
        {
            if (Random.value <= 0.2f || marker_flag[i])
            {
                // 確率20%でマーカを有効化
                marker_flag[i] = true;
                num_of_pointMarker++;
                GameObject obj = Instantiate(PointMarker, new Vector3(marker_positison_x[i], 0.0f, 0.0f), Quaternion.identity) as GameObject;
            }
        }

        if (num_of_pointMarker > 6) bluewall.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
