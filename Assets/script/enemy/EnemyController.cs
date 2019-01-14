using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static float walkingSpeed = 2.0f;
    public static float r = 2.0f;
    public static MonoBehaviour monoBehaviour;
    Dictionary<string, int> key = new Dictionary<string, int>();
    Dictionary<string, float> seconds = new Dictionary<string, float>();
    public static Dictionary<string, GameObject> mDeadEnemy = new Dictionary<string, GameObject>();
    public GameObject bullet;//プレハブを取得


    void Start()
    {
        monoBehaviour = this;

        //最初の向きをランダムに決める
        System.Random rnd = new System.Random();
        int intResult = rnd.Next(100);
        if (intResult > 50)
        {
            key[gameObject.name] = -1;
        }
        else
        {
            key[gameObject.name] = 1;
        }

        //敵が増えるたびに秒数のカウントが早くなるので、敵ごとにカウントを分ける
        seconds[gameObject.name] = 0;

        //初期化
        mDeadEnemy[gameObject.name] = null;
    }

    void Update()
    {

        //nullチェック
        if (!mDeadEnemy[gameObject.name])
        {
            if (gameObject.tag.Equals("plant"))
            {
                PlantMotion();
            }
            else
            {
                Walk();
            }
        }
        else
        {
            //死んだenemyと今のenemyがちがければ歩く
            if (!gameObject.transform.name.Equals(mDeadEnemy[gameObject.name].transform.name))
            {
                if (gameObject.tag.Equals("plant"))
                {
                    PlantMotion();
                }
                else
                {
                    Walk();
                }

            }
        }
    }

    public void Walk()
    {
        Rigidbody2D rigid2D = GetComponent<Rigidbody2D>();

        //タイマーで周期的に反転させる
        seconds[gameObject.name] += Time.deltaTime;

        if (seconds[gameObject.name] > 3.0f)
        {
            seconds[gameObject.name] = 0;
            key[gameObject.name] *= -1;

        }

        //移動
        rigid2D.velocity = new Vector3(walkingSpeed * key[gameObject.name], rigid2D.velocity.y, 0);

        //左右反転
        float x = Mathf.Abs(rigid2D.transform.localScale.x) * -key[gameObject.name];
        float y = rigid2D.transform.localScale.y;
        rigid2D.transform.localScale = new Vector2(x, y);
    }

    public void PlantMotion()
    {
        //死亡判定
        if (mDeadEnemy[gameObject.name]) return;

        //タイマーで周期的に反転させる
        seconds[gameObject.name] += Time.deltaTime;

        AnimatorStateInfo animatorStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        if (seconds[gameObject.name] < 3.0f)
        {
            if (!animatorStateInfo.IsName("idle"))
            {
                GetComponent<Animator>().SetTrigger("IdleTrigger");
            }
        }

        if (seconds[gameObject.name] > 3.0f)
        {
            if (!animatorStateInfo.IsName("attack"))
            {
                GetComponent<Animator>().SetTrigger("AttackTrigger");
            }
        }

        if (seconds[gameObject.name] > 3.9f)
        {
            float x = gameObject.transform.position.x - 0.5f;
            float y = gameObject.transform.position.y;
            float z = gameObject.transform.position.z;
            Instantiate(bullet, new Vector3(x, y, z), Quaternion.identity);
            seconds[gameObject.name] = 0.0f;
        }

    }


    public static void Death(GameObject deadEnemy)
    {
        mDeadEnemy[deadEnemy.name] = deadEnemy;

        //コンポーネント取得
        Rigidbody2D rigid2D = deadEnemy.GetComponent<Rigidbody2D>();
        Animator animator = deadEnemy.GetComponent<Animator>();

        //速度を止める
        rigid2D.velocity = new Vector3(0, 0, 0);

        //大きさを半分にする
        float x = rigid2D.transform.localScale.x;
        float y = rigid2D.transform.localScale.y / r;
        rigid2D.transform.localScale = new Vector2(x, y);

        //その場に固定
        deadEnemy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;


        //アニメーションを止める
        animator.speed = 0;

        //一秒後にdeathアニメーションを呼ぶ
        monoBehaviour.StartCoroutine(DelayMethod(0.8f, () =>
        {
            DeathAnimationTrigger(deadEnemy);
        }));

    }



    public static void DeathAnimationTrigger(GameObject deadEnemy)
    {
        Animator animator = deadEnemy.GetComponent<Animator>();
        animator.speed = 1.0f;
        animator.SetTrigger("DeathTrigger");

        BoxCollider2D[] boxCollider2Ds = deadEnemy.GetComponentsInChildren<BoxCollider2D>();
        CircleCollider2D circleCollider2D = deadEnemy.GetComponent<CircleCollider2D>();

        //コライダをすべてオフにする
        if (circleCollider2D) circleCollider2D.enabled = false;
        foreach (BoxCollider2D boxCollider2D in boxCollider2Ds)
        {
            boxCollider2D.enabled = false;
        }

        //数秒後に消す
        monoBehaviour.StartCoroutine(DelayMethod(0.8f, () =>
        {
            deadEnemy.transform.localScale = new Vector3(0, 0, 0);
        }));

    }

    public static IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

}
