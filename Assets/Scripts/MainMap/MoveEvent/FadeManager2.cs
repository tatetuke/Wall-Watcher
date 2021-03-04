using UnityEngine;
using System.Collections;
/// <summary>
/// シーン遷移無しのフェードイン・アウトを制御し、その間に指定のオブジェクトの移動を制御するためのクラス
/// </summary>
public class FadeManager2 : SingletonMonoBehaviour<FadeManager2>
{
    /// <summary>暗転用黒テクスチャ</summary>
    private Texture2D blackTexture;
    /// <summary>フェード中の透明度</summary>
    private float fadeAlpha = 0;
    /// <summary>フェード中かどうか</summary>
    private bool isFading = false;

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        //DontDestroyOnLoad(this.gameObject);

        //ここで黒テクスチャ作る
        this.blackTexture = new Texture2D(32, 32, TextureFormat.RGB24, false);
        this.blackTexture.ReadPixels(new Rect(0, 0, 32, 32), 0, 0, false);
        this.blackTexture.SetPixel(0, 0, Color.white);
        this.blackTexture.Apply();
    }

    public void OnGUI()
    {
        if (!this.isFading)
            return;

        //透明度を更新して黒テクスチャを描画
        GUI.color = new Color(0, 0, 0, this.fadeAlpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), this.blackTexture);
    }

    /// <summary>
    /// 画面遷移
    /// </summary>
    /// <param name='interval'>暗転にかかる時間(秒)</param>
    /// <param name="obj">移動するオブジェクト</param>
    /// <param name="x">移動先の x 座標</param>
    /// <param name="y">移動先の y 座標</param>
    public void LoadLevel2(float interval, GameObject obj, float x, float y)
    {
        StartCoroutine(TransScene(interval, obj, x, y));
    }


    /// <summary>
    /// シーン遷移用コルーチン
    /// </summary>
    /// <param name='interval'>暗転にかかる時間(秒)</param>
    /// <param name="obj">移動するオブジェクト</param>
    /// <param name="x">移動先の x 座標</param>
    /// <param name="y">移動先の y 座標</param>
    private IEnumerator TransScene(float interval, GameObject obj, float x, float y)
    {
        //だんだん暗く
        this.isFading = true;
        float time = 0;
        while (time <= interval)
        {
            this.fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
            time += Time.deltaTime;
            yield return 0;
        }

        // オブジェクトの移動
        obj.transform.position = new Vector3(x, y, 0);

        //だんだん明るく
        time = 0;
        while (time <= interval)
        {
            this.fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
            time += Time.deltaTime;
            yield return 0;
        }

        this.isFading = false;
    }

}