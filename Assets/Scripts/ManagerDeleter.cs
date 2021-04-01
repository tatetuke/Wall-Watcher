using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerDeleter : MonoBehaviour
{
    [Tooltip("大きければ大きいほど優先される。同じなら未定義")]
    [SerializeField] int priority = 0;
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        var gameManagers = FindObjectsOfType<ManagerDeleter>();
        var max_gameObject=gameObject;
        int max = -99999;
        foreach (var i in gameManagers)
        {
            if (i.priority >= max)
            {
                if (i.priority == max)
                {
                    Debug.LogWarning("same priority deleters are existing.");
                }
                max = i.priority;
                max_gameObject = i.gameObject;
            }
        }
        foreach(var i in gameManagers)
        {
            if (i.gameObject != max_gameObject)
            {
                Destroy(i.gameObject);
            }
        }
    }
}
