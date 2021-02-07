using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DbgConsole
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DbgRed(string message)
    {
        Debug.Log($"<color=#ff0000>{message}</color>");
    }
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DbgBlue(string message)
    {
        Debug.Log($"<color=#0000ff>{message}</color>");
    }
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DbgGreen(string message)
    {
        Debug.Log($"<color=#00ff00>{message}</color>");
    }
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DbgYellow(string message)
    {
        Debug.Log($"<color=#ffff00>{message}</color>");
    }
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DbgCyan(string message)
    {
        Debug.Log($"<color=#00ffff>{message}</color>");
    }
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DbgPurple(string message)
    {
        Debug.Log($"<color=#ff00ff>{message}</color>");
    }
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Dbg(string message)
    {
        Debug.Log(message);
    }


    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DbgRed(string message,Object obj)
    {
        Debug.Log($"<color=#ff0000>{message}</color>", obj);
    }
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DbgBlue(string message, Object obj)
    {
        Debug.Log($"<color=#0000ff>{message}</color>", obj);
    }
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DbgGreen(string message, Object obj)
    {
        Debug.Log($"<color=#00ff00>{message}</color>", obj);
    }
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DbgYellow(string message, Object obj)
    {
        Debug.Log($"<color=#ffff00>{message}</color>", obj);
    }
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DbgCyan(string message, Object obj)
    {
        Debug.Log($"<color=#00ffff>{message}</color>", obj);
    }
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DbgPurple(string message, Object obj)
    {
        Debug.Log($"<color=#ff00ff>{message}</color>", obj);
    }
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Dbg(string message, Object obj)
    {
        Debug.Log(message, obj);
    }
}
