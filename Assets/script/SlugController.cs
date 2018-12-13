using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugController : MonoBehaviour
{

    Rigidbody2D rigid2D;
    Animator animator;
    float walkingSpeed = 2.0f;
    int key = -1;
    float seconds;
    bool isDead;
    CircleCollider2D circleCollider2D;
    float r = 2.0f;

    string Player = "Player";
    string slugLR = "slugLR";
    string slugHead = "slugHead";
    string playerFoot = "playerFoot";




    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        circleCollider2D = GetComponent<CircleCollider2D>();

    }

    void Update()
    {

        if (!isDead) Walk();

    }

    void Walk()
    {
        //タイマーで周期的に反転させる
        seconds += Time.deltaTime;
        //Debug.Log(seconds);

        if (seconds > 3.0f)
        {
            seconds = 0;
            key *= -1;
        }

        //移動
        rigid2D.velocity = new Vector3(walkingSpeed * key, rigid2D.velocity.y, 0);

        //左右反転
        float x = Mathf.Abs(transform.localScale.x) * -key;
        float y = transform.localScale.y;
        transform.localScale = new Vector2(x, y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //衝突を検知したのが"playerFoot"なら死ぬ
        if (other.gameObject.tag.Equals(playerFoot)) Death();
    }

    void Death()
    {
        //一度衝突を検知していたら何もしない
        if (isDead) return;

        //衝突したかどうか
        isDead = true;

        //速度を止める
        rigid2D.velocity = new Vector3(0, 0, 0);

        //大きさを半分にする
        float x = transform.localScale.x;
        float y = transform.localScale.y / r;
        transform.localScale = new Vector2(x, y);

        //コライダの大きさも半分にする
        circleCollider2D.radius = circleCollider2D.radius / r;

        //アニメーションを止める
        animator.speed = 0;

        //一秒後にdeathアニメーションを呼ぶ
        StartCoroutine(DelayMethod(0.8f, () =>
        {
            DeathAnimationTrigger("DeathTrigger");
        }));

    }

    //TODO:インターフェイスにする
    public void DeathAnimationTrigger(string parameters)
    {
        animator.speed = 1.0f;
        animator.SetTrigger(parameters);

        //一秒後にオブジェクトを破棄する
        StartCoroutine(DelayMethod(0.8f, () =>
        {
            Destroy(gameObject);
        }));
    }

    IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }





}
