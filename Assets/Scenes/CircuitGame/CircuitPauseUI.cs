using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CircuitPauseUI : MonoBehaviour
{

    Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {

        var gameManager = FindObjectOfType<CircuitGameManager>();
        gameManager.OnGamePause.AddListener(()=> {
            animator.Play("CircuitPauseUIFadeIn");
        });
        gameManager.OnGameResume.AddListener(() =>
        {
            animator.Play("CircuitPauseUIFadeOut");
        });
    }

    // Update is called once per frame
    void Update()
    {
    }
}
