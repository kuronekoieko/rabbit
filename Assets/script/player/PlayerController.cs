using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{

    public static Rigidbody2D rigid2D;
    public static float jumpYForce = 900.0f;
    public static float walkSpeed = 10.0f;
    public static Vector3 buttonDownPosition;
    public static Vector3 buttonPosition;
    public static Vector3 buttonRereasePosition;
    public static Vector3 flickVector;
    public static int key;
    public static Animator animator;
    public static float nearyZero = 0.1f;
    public static float v1;
    public static float v2;
    public static float a;
    public static int count;
    public static float sin;
    public static bool isMouseRerease = true;
    public static Vector2 playerPosition;
    public static AnimatorStateInfo stateInfo;
    public static bool isAbleToJump;
    public static bool isCollisionStay;
    public static bool isJumpNow;
    public static bool isLaddering;
    public static float ladderingSpeed = 3.0f;
    public static int keyX;
    public static int keyY;
    public static bool isCollisionSlug;
    public static float seconds;
    public static bool isHurting;

    public static string ladder = "ladder";
    public static string slug = "slug";
    public static string Tilemap = "Tilemap";
    public static string slugHead = "slugHead";


    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        //加速度計測
        Accelalation();

        //ジャンプの状態を取得
        JumpCounter();

        //ジャンプできる様になる条件
        isAbleToJump |= (isCollisionStay && isMouseRerease);

        //ジャンプアニメーション
        if (a < nearyZero && rigid2D.velocity.y > 0 && isJumpNow) PlayerAmination.JumpAnim();

        //攻撃をうけたときのアニメーション
        if (isHurting) PlayerAmination.HurtAnim();

        //落下時アニメーション
        if (!isLaddering && rigid2D.velocity.y < -2 && !isHurting) PlayerAmination.FallAnim();


        //フリック時の動作
        if (isLaddering)
        {
            //はしごを登ってるとき
            FlickLaddering();
            animator.speed = keyX == 0 && keyY == 0 ? 0f : 1f;
        }
        else
        {
            //通常時
            if (!isHurting) Flick();

        }

        //落ちたら戻る
        if (transform.position.y < -50) SceneManager.LoadScene("Stage1");

    }

    public static void FlickLaddering()
    {
        //タッチした瞬間の動作
        if (Input.GetMouseButtonDown(0))
        {
            isMouseRerease = false;
            buttonDownPosition = Input.mousePosition;
        }

        //タッチ中の動作
        if (Input.GetMouseButton(0))
        {

            //はしごの移動
            buttonPosition = Input.mousePosition;

            //フリックしている向きのベクトルを取得
            flickVector = buttonPosition - buttonDownPosition;


            //x方向の向きを見て、方向を判定する
            if (flickVector.x > 0)
            {
                keyX = 1;
            }
            else if (flickVector.x < 0)
            {
                keyX = -1;
            }


            //y方向の向きを見て、方向を判定する
            if (flickVector.y > 0)
            {
                keyY = 1;
            }
            else if (flickVector.y < 0)
            {
                keyY = -1;
            }


            sin = flickVector.y / flickVector.magnitude;

            float x = keyX * ladderingSpeed;
            float y = keyY * ladderingSpeed;

            rigid2D.velocity = Mathf.Abs(sin) > 0.71f ? new Vector2(0, y) : new Vector2(x, 0);

        }

        //タッチを離したときの処理
        if (Input.GetMouseButtonUp(0))
        {
            keyX = 0;
            keyY = 0;
            buttonPosition.Set(0, 0, 0);
            buttonDownPosition.Set(0, 0, 0);
            rigid2D.velocity = new Vector3(0, 0, 0);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag.Equals(ladder))
        {
            //はしごに入った瞬間に呼ばれる
            isJumpNow = false;
            isLaddering = true;
            PlayerAmination.ClimbAnim();
            rigid2D.gravityScale = 0;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals(ladder))
        {
            //はしごを抜けた瞬間に呼ばれる
            rigid2D.gravityScale = 5;
            isLaddering = false;
        }

        if (other.gameObject.tag.Equals(slugHead))
        {
            isCollisionStay = false;

            //slugから離れたときの処理
            isCollisionSlug &= !other.gameObject.tag.Equals(slug);
        }

    }

    void OnCollisionStay2D(Collision2D other)
    {
        //タイルマップに接触してる間に呼ばれる
        if (other.gameObject.name.Equals(Tilemap))
        {
            isCollisionStay = true;

            //止まったらidle
            if (key == 0) PlayerAmination.IdleAnim();

            //スキップアニメーション
            if (!isLaddering && key != 0) PlayerAmination.SkipAnim();
        }


    }


    void OnCollisionExit2D(Collision2D collision)
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        //Tilemapに衝突したときの処理
        if (collision.gameObject.name.Equals(Tilemap))
        {
            isHurting = false;
            isJumpNow = false;
            isCollisionStay = true;
        }
    }

    public static void CollisionSlug()
    {
        if (isCollisionSlug) return;

        isCollisionSlug = true;
        rigid2D.AddForce(rigid2D.transform.up * jumpYForce * 2.0f);
        PlayerAmination.JumpAnim();
    }

    public static void Hurt(float hurtKey)
    {

        if (isHurting) return;
        isHurting = true;

        rigid2D.velocity = new Vector3(0, 0, 0);
        rigid2D.AddForce(rigid2D.transform.up * jumpYForce);
        rigid2D.velocity = new Vector3(3.0f * hurtKey, rigid2D.velocity.y, 0);

    }


    //画面をタップしてから離すまでのフロー
    public static void Flick()
    {

        //タップした始点と、今タップしてる点のベクトルを取得
        if (Input.GetMouseButtonDown(0))
        {
            isMouseRerease = false;
            buttonDownPosition = Input.mousePosition;
        }

        //タッチ中の動作
        if (Input.GetMouseButton(0)) FlickMotion();

        //タップを離したときに処理
        if (Input.GetMouseButtonUp(0)) FlickEnd();

    }


    public static void FlickMotion()
    {

        buttonPosition = Input.mousePosition;

        //フリックしている向きのベクトルを取得
        flickVector = buttonPosition - buttonDownPosition;

        //タップしている間の時間を計測
        seconds += Time.deltaTime;

        //x方向の向きを見て、方向を判定する
        if (flickVector.x > 0)
        {
            key = 1;
        }
        else if (flickVector.x < 0)
        {
            key = -1;
        }
        else
        {
            key = 0;
        }

        //移動
        Walk();
    }

    public static void FlickEnd()
    {
        //タップを離したときのジャンプ
        TappingJump();

        //以下初期化
        isMouseRerease = true;
        key = 0;
        buttonPosition.Set(0, 0, 0);
        buttonDownPosition.Set(0, 0, 0);
        rigid2D.velocity = new Vector2(0, rigid2D.velocity.y);
        seconds = 0;
    }


    public static void FlickJump()
    {
        //ジャンプ動作
        if (Mathf.Abs(a) < nearyZero && IsNotJump() && isAbleToJump)
        {
            rigid2D.AddForce(rigid2D.transform.up * jumpYForce);
            isAbleToJump = false;
            isJumpNow = true;
        }
    }

    public static void TappingJump()
    {
        buttonRereasePosition = Input.mousePosition;
        Vector3 tappingVector = buttonRereasePosition - buttonDownPosition;

        float distance2 = Mathf.Pow(tappingVector.x, 2) + Mathf.Pow(tappingVector.y, 2) + Mathf.Pow(tappingVector.z, 2);
        float distance = Mathf.Sqrt(distance2);

        if (Mathf.Abs(distance) < 1.0f && IsNotJump() && isAbleToJump && seconds < 0.2f)
        {
            rigid2D.AddForce(rigid2D.transform.up * jumpYForce);
            isAbleToJump = false;
            isJumpNow = true;
        }
    }


    public static void Walk()
    {
        if (isHurting) return;

        //歩いてるときは等速で移動
        rigid2D.velocity = new Vector2(walkSpeed * key, rigid2D.velocity.y);

        float x = Mathf.Abs(rigid2D.transform.localScale.x) * key;
        float y = rigid2D.transform.localScale.y;
        if (key != 0) rigid2D.transform.localScale = new Vector2(x, y);

    }


    public static void JumpCounter()
    {

        //着地時count=1
        //ジャンプ時count=2
        if (a > nearyZero)
        {
            count++;
            if (count % 2 == 0)
            {
                //Debug.Log("ジャンプ時" + count);
                count = 0;
            }
        }
    }

    public static bool IsNotJump()
    {
        return Mathf.Abs(rigid2D.velocity.y) < nearyZero;
    }

    public static bool IsStateEquals(string clip)
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //Debug.Log(stateInfo.IsName(clip));
        return stateInfo.IsName(clip);
    }

    public static void Accelalation()
    {
        //現在の速度を取得
        v2 = rigid2D.velocity.y;

        //加速度を計算
        a = (v2 - v1) / Time.deltaTime;

        //現在の速度を、前のフレームの速度としてする
        v1 = v2;

    }


}

