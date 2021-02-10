using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class UsefulFunctions : MonoBehaviour
{

    //originaiを複製する関数
    //座標や向きは設定されていない(0のまま)
    public static GameObject CloneObject(GameObject original)
    {
        return Instantiate(original, Vector3.zero, Quaternion.identity);
    }

    //親のScaleに依存せず、グローバルScaleのもとでオブジェクトを移動させる
    public static void TransformNonScale(ref Transform transform, Vector3 offset)
    {
        Vector3 lossScale = transform.lossyScale;
        transform.position += new Vector3(offset.x / lossScale.x,
            offset.y / lossScale.y,
            offset.z / lossScale.z);
    }

    public static void SceneChange(String sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void RotateArrayClockwise<Type>(Type[,] array)
    {
        // 引数の2次元配列 array を時計回りに回転させたものを返す
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);
        var t = new Type[cols, rows];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                t[j, rows - i - 1] = array[i, j];
            }
        }
    }
    public static void RotateArrayAnticlockwise<Type>(Type[,] array)
    {
        // 引数の2次元配列 array を反時計回りに回転させたものを返す
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);
        var t = new Type[cols, rows];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                t[cols - j - 1, i] = array[i, j];
            }
        }
    }

    public static Vector2 Rotate(Vector2 vec, float angleRad)
    {
        float sin = Mathf.Sin(angleRad), cos = Mathf.Cos(angleRad);

        var x = vec.x * cos - vec.y * sin;
        var y = vec.x * sin + vec.y * cos;
        return new Vector2(x, y);
    }

    //DXLibみたいな
    public static void TransformGenerator2D(ref Transform transform, Vector3 potition, float rateX, float rateY, float rotateAngle)
    {
        transform.position = potition;
        transform.localScale = new Vector3(rateX, rateY, 1f);
        transform.Rotate(0f, 0f, rotateAngle);
    }

    /*  bool IsInArray(Vector3Int[] array, Vector3Int cood)
      {
          foreach (var i in array)
              if (cood == i) return true;
          return false;
      }
      bool IsInArray(Vector3Int[,] array, Vector3Int cood)
      {
          foreach (var i in array)
              if (cood == i) return true;
          return false;
      }*/

    public static Vector2 Polar(float thetaRad)
    {
        return new Vector2(Mathf.Cos(thetaRad), Mathf.Sin(thetaRad));
    }
    public static Vector2 Polar(float r, float thetaRad)
    {
        return new Vector2(r * Mathf.Cos(thetaRad), r * Mathf.Sin(thetaRad));
    }
}

public struct MyMath
{
    public static uint Clamp(uint value, uint min, uint max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }
}


public static class UsefulExtensions
{
    public static void Rotate(this Vector2 v, float angleRad)
    {
        float sin = Mathf.Sin(angleRad), cos = Mathf.Cos(angleRad);
        var x = v.x * cos - v.y * sin;
        var y = v.x * sin + v.y * cos;
        v.x = x; v.y = y;
    }
}