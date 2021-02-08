using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class MonoBehaviorExtentsion
{
    public static IEnumerator DelayMethod<T>(this MonoBehaviour mono, float waitTime, Action<T> action, T t)
    {
        yield return new WaitForSeconds(waitTime);
        action(t);
    }

    public static IEnumerator DelayMethod(this MonoBehaviour mono, float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    public static Coroutine Delay<T>(this MonoBehaviour mono, float waitTime, Action<T> action, T t)
    {
        return mono.StartCoroutine(DelayMethod(mono, waitTime, action, t));
    }

    public static Coroutine Delay(this MonoBehaviour mono, float waitTime, Action action)
    {
        return mono.StartCoroutine(DelayMethod(mono, waitTime, action));
    }

}