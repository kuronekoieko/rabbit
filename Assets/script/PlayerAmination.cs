using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimState
{
    idle,
    climb,
    fall,
    jump,
    skip
}

public enum AnimTrigger
{
    JumpTrigger,
    SkipTrigger,
    IdleTrigger,
    FallTrigger,
    ClimbTrigger
}


public class PlayerAmination : MonoBehaviour {

    static Animator animator = PlayerController.animator;
    GameObject player;

    // Use this for initialization
    void Start () {
        player = GetComponent<GameObject>();
        //animator = PlayerController.animator;
    }
	
	// Update is called once per frame
    void Update () {
		
	}


    public static void JumpTrigger()
    {
        //連続再生で止まってるように見えるのをを防ぐ
        if (IsAnimState(AnimState.jump)) return;
        Animation(AnimTrigger.JumpTrigger);
        //Debug.Log(parameters);
    }

    public static void SkipTrigger()
    {
        //連続再生で止まってるように見えるのをを防ぐ
        if (IsAnimState(AnimState.skip)) return;
        Animation(AnimTrigger.SkipTrigger);
        //Debug.Log(parameters);
    }

    public static void IdleTrigger()
    {
        //連続再生で止まってるように見えるのをを防ぐ
        if (IsAnimState(AnimState.idle)) return;
        Animation(AnimTrigger.IdleTrigger);
        //Debug.Log(parameters);
    }


    public static void FallTrigger()
    {
        //連続再生で止まってるように見えるのをを防ぐ
        if (IsAnimState(AnimState.fall)) return;
        Animation(AnimTrigger.FallTrigger);
        //Debug.Log(parameters);
    }


    public static void ClimbTrigger()
    {
        //連続再生で止まってるように見えるのをを防ぐ
        if (IsAnimState(AnimState.climb)) return;
        Animation(AnimTrigger.ClimbTrigger);
        //Debug.Log(parameters);
    }

    static void Animation(AnimTrigger animTrigger)
    {
        string param = animTrigger.ToString();
        animator.SetTrigger(param);
        //Debug.Log(param);
    }

    public static bool IsAnimState(AnimState state)
    {
        //今のアニメのstartが、引数のアニメと同じならtrueを返す
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool isAnimThis = animatorStateInfo.IsName(state.ToString());
        return isAnimThis;

    }



}

