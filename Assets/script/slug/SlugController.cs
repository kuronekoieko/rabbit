using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugController : MonoBehaviour
{

    public static Animator animator;
    public static float walkingSpeed = 2.0f;
    public static int key = -1;
    public static float seconds;
    public static float r = 2.0f;
    public static MonoBehaviour monoBehaviour;
    public static GameObject mDeadSlug;

    void Start()
    {
        monoBehaviour = this;
    }

    void Update()
    {
        //nullチェック
        if (!mDeadSlug)
        {
            Walk();
        }
        else
        {
            //死んだslugと今のslugがちがければ歩く
            if (!gameObject.transform.name.Equals(mDeadSlug.transform.name)) Walk();
        }
    }

    public void Walk()
    {

        Rigidbody2D rigid2D = GetComponent<Rigidbody2D>();
        //タイマーで周期的に反転させる
        seconds += Time.deltaTime;

        if (seconds > 3.0f)
        {
            seconds = 0;
            key *= -1;
        }

        //移動
        rigid2D.velocity = new Vector3(walkingSpeed * key, rigid2D.velocity.y, 0);

        //左右反転
        float x = Mathf.Abs(rigid2D.transform.localScale.x) * -key;
        float y = rigid2D.transform.localScale.y;
        rigid2D.transform.localScale = new Vector2(x, y);
    }


    public static void Death(GameObject deadSlug)
    {
        mDeadSlug = deadSlug;
        Rigidbody2D rigid2D = deadSlug.GetComponent<Rigidbody2D>();
        CircleCollider2D circleCollider2D = deadSlug.GetComponent<CircleCollider2D>();
        animator = deadSlug.GetComponent<Animator>();

        //速度を止める
        rigid2D.velocity = new Vector3(0, 0, 0);

        //大きさを半分にする
        float x = rigid2D.transform.localScale.x;
        float y = rigid2D.transform.localScale.y / r;
        rigid2D.transform.localScale = new Vector2(x, y);

        //コライダの大きさも半分にする
        circleCollider2D.radius = circleCollider2D.radius / r;

        //アニメーションを止める
        animator.speed = 0;

        //一秒後にdeathアニメーションを呼ぶ
        monoBehaviour.StartCoroutine(DelayMethod(0.8f, () =>
        {
            DeathAnimationTrigger(deadSlug);
        }));

    }



    public static void DeathAnimationTrigger(GameObject deadSlug)
    {
        animator.speed = 1.0f;
        animator.SetTrigger("DeathTrigger");

        //数秒後に消す
        monoBehaviour.StartCoroutine(DelayMethod(0.8f, () =>
        {
            deadSlug.transform.localScale = new Vector3(0, 0, 0);
        }));

    }

    public static IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }



}
