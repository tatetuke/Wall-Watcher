using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerDeleter : MonoBehaviour
{
    [SerializeField] string winner_name = "Managers_Strong";
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        var gameManagers = FindObjectsOfType<Kyoichi.GameManager>();
        foreach(var i in gameManagers)
        {
            if(i.gameObject.name!= winner_name)
            {
                Destroy(i.gameObject);
            }
        }
    }
}
