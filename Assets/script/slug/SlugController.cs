using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugController : MonoBehaviour
{

    public static Rigidbody2D rigid2D;
    public static Animator animator;
    public static float walkingSpeed = 2.0f;
    public static int key = -1;
    public static float seconds;
    public static bool isDead;
    public static CircleCollider2D circleCollider2D;
    public static float r = 2.0f;

    public static string Player = "Player";
    public static string slugLR = "slugLR";
    public static string slugHead = "slugHead";
    public static MonoBehaviour monoBehaviour;

    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        monoBehaviour = this;
    }

    void Update()
    {

        if (!isDead) Walk();

    }

    public static void Walk()
    {
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


    public static void Death()
    {
        //一度衝突を検知していたら何もしない
        if (isDead) return;

        //衝突したかどうか
        isDead = true;

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
            DeathAnimationTrigger("DeathTrigger");
        }));

    }



    public static void DeathAnimationTrigger(string parameters)
    {
        animator.speed = 1.0f;
        animator.SetTrigger(parameters);

        //一秒後にオブジェクトを破棄する
        monoBehaviour.StartCoroutine(DelayMethod(0.8f, () =>
        {
            Destroy(monoBehaviour.gameObject);
        }));
    }

    public static IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }



}
