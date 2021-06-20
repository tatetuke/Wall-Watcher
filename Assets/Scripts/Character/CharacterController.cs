//https://qiita.com/narumi_/items/182ce9f62c45fc2a6bdb
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed;

    Animator animator;
    float movementInWalk;

    private static readonly int walkNameHash = Animator.StringToHash("Player_Walk");

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();

        movementInWalk = ExamineMovement(walkNameHash);

        Walk();
    }

    void OnAnimatorMove()
    {
        // Update movement in animation
        var deltaPos = animator.deltaPosition;
        //transform.localPosition += deltaPos;
        //transform.position += deltaPos;
        //transform.position += new Vector3(1,0,0);
        //Debug.Log(deltaPos);
    }

    // Measure the amount of movement to the Z axis during animation
    private float ExamineMovement(int nameHash)
    {
        animator.Play(nameHash);
        animator.Update(0f);

        var currentClip = animator.GetCurrentAnimatorClipInfo(layerIndex: 0)[0].clip;
        animator.Update(currentClip.length);

        var movement = Vector3.Project(animator.deltaPosition, new Vector3(1,0,0)).magnitude / currentClip.length;
        return movement;
    }

    private void Walk()
    {
        animator.speed = moveSpeed / movementInWalk; // Adjust the animation speed according to the moving speed
        animator.PlayInFixedTime(walkNameHash, layer: 0, fixedTime: 0f);

    }


}
