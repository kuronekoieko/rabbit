using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmination : MonoBehaviour
{
    //アニメーションのトリガー名
    public static string JumpTrigger = "JumpTrigger";
    public static string SkipTrigger = "SkipTrigger";
    public static string IdleTrigger = "IdleTrigger";
    public static string FallTrigger = "FallTrigger";
    public static string ClimbTrigger = "ClimbTrigger";
    public static string HurtTrigger = "HurtTrigger";

    //アニメーションのステート名
    public static string idle = "idle";
    public static string climb = "climb";
    public static string fall = "fall";
    public static string jump = "jump";
    public static string skip = "skip";
    public static string hurt = "hurt";

    static Animator animator = PlayerController.animator;

    void Start()
    {

    }


    void Update()
    {

    }


    public static void JumpAnim()
    {
        //連続再生で止まってるように見えるのをを防ぐ
        if (IsAnimState(jump)) return;
        animator.SetTrigger(JumpTrigger);
        //Debug.Log(parameters);
    }

    public static void SkipAnim()
    {
        //連続再生で止まってるように見えるのをを防ぐ
        if (IsAnimState(skip)) return;
        animator.SetTrigger(SkipTrigger);
        //Debug.Log(parameters);
    }

    public static void IdleAnim()
    {
        //連続再生で止まってるように見えるのをを防ぐ
        if (IsAnimState(idle)) return;
        animator.SetTrigger(IdleTrigger);
        //Debug.Log(parameters);
    }


    public static void FallAnim()
    {
        //連続再生で止まってるように見えるのをを防ぐ
        if (IsAnimState(fall)) return;
        animator.SetTrigger(FallTrigger);
        //Debug.Log(parameters);
    }


    public static void ClimbAnim()
    {
        //連続再生で止まってるように見えるのをを防ぐ
        if (IsAnimState(climb)) return;
        animator.SetTrigger(ClimbTrigger);
        //Debug.Log(parameters);
    }

    public static void HurtAnim()
    {
        // 連続再生で止まってるように見えるのをを防ぐ
        if (IsAnimState(hurt)) return;
        animator.SetTrigger(HurtTrigger);
    }

    public static bool IsAnimState(string state)
    {
        //今のアニメのstartが、引数のアニメと同じならtrueを返す
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return animatorStateInfo.IsName(state);

    }



}

