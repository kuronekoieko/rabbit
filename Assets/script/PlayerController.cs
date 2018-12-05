using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{

    Rigidbody2D rigid2D;
    float jumpYForce = 900.0f;
    float walkSpeed = 10.0f;
    Vector3 buttonDownPosition;
    Vector3 buttonPosition;
    Vector3 buttonRereasePosition;
    Vector3 flickVector;
    int key;
    public static Animator animator;
    float nearyZero = 0.1f;//1E-04
    float v1;
    float v2;
    float a;
    int count;
    float sin;
    bool isMouseRerease = true;
    public Vector2 playerPosition;
    AnimatorStateInfo stateInfo;
    bool isAbleToJump;
    bool isCollisionStay;
    bool isJumpNow;
    bool isLaddering;
    float ladderingSpeed = 3.0f;
    int keyX;
    int keyY;
    bool isCollisionSlug;
    float seconds;

    // Use this for initialization
    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }



    // Update is called once per frame
    void Update()
    {


        //加速度計測
        Accelalation();

        //ジャンプの状態を取得
        JumpCounter();

        //ジャンプできる様になる条件
        isAbleToJump |= (isCollisionStay && isMouseRerease);

        //ジャンプアニメーション
        if (a < nearyZero && rigid2D.velocity.y > 0 && isJumpNow)
        {
            PlayerAmination.JumpTrigger();
        }

        //落下時アニメーション
        if (!isLaddering && rigid2D.velocity.y < -2) PlayerAmination.FallTrigger();


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
            Flick();
        }

        //落ちたら戻る
        if (transform.position.y < -50) SceneManager.LoadScene("Stage1");

    }

    void FlickLaddering()
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
        //はしごに入った瞬間に呼ばれる
        isJumpNow = false;
        isLaddering = true;
        PlayerAmination.ClimbTrigger();
        rigid2D.gravityScale = 0;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //はしごを抜けた瞬間に呼ばれる
        rigid2D.gravityScale = 5;
        isLaddering = false;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        //タイルマップに接触してる間に呼ばれる

        isCollisionStay = true;

        //止まったらidle
        if (key == 0) PlayerAmination.IdleTrigger();

        //スキップアニメーション
        if (!isLaddering && key != 0) PlayerAmination.SkipTrigger();

    }


    void OnCollisionExit2D(Collision2D collision)
    {

        isCollisionStay = false;

        //slugから離れたときの処理
        isCollisionSlug &= !collision.gameObject.tag.Equals("slug");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        //Tilemapに衝突したときの処理
        if (collision.gameObject.name.Equals("Tilemap"))
        {
            isJumpNow = false;
            isCollisionStay = true;
        }


        //slugに衝突したときの動作
        if (collision.gameObject.tag.Equals("slug")) CollisionSlug();

    }

    void CollisionSlug()
    {

        if (isCollisionSlug) return;


        isCollisionSlug = true;
        rigid2D.AddForce(transform.up * jumpYForce*1.4f);
        PlayerAmination.JumpTrigger();
    }


    //画面をタップしてから離すまでのフロー
    void Flick()
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


    void FlickMotion()
    {

        buttonPosition = Input.mousePosition;

        //フリックしている向きのベクトルを取得
        flickVector = buttonPosition - buttonDownPosition;
        //flickVector = new Vector3(0, flickVector.y, flickVector.z);

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

    void FlickEnd()
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


    void FlickJump()
    {
        //ジャンプ動作
        if (Mathf.Abs(a) < nearyZero && IsNotJump() && isAbleToJump)
        {
            rigid2D.AddForce(transform.up * jumpYForce);
            isAbleToJump = false;
            isJumpNow = true;
        }
    }

    void TappingJump()
    {
        buttonRereasePosition = Input.mousePosition;
        Vector3 tappingVector = buttonRereasePosition - buttonDownPosition;

        float distance2 = Mathf.Pow(tappingVector.x, 2) + Mathf.Pow(tappingVector.y, 2) + Mathf.Pow(tappingVector.z, 2);
        float distance = Mathf.Sqrt(distance2);

        if (Mathf.Abs(distance) < 1.0f && IsNotJump() && isAbleToJump && seconds < 0.2f) {
            rigid2D.AddForce(transform.up * jumpYForce);
            isAbleToJump = false;
            isJumpNow = true;
        }
    }


    void Walk()
    {

        //歩いてるときは等速で移動
        rigid2D.velocity = new Vector2(walkSpeed * key, rigid2D.velocity.y);

        float x = Mathf.Abs(transform.localScale.x) * key;
        float y = transform.localScale.y;
        if (key != 0) transform.localScale = new Vector2(x, y);

    }


    void JumpCounter()
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

    bool IsNotJump()
    {
        return Mathf.Abs(rigid2D.velocity.y) < nearyZero;
    }

    bool IsStateEquals(string clip)
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //Debug.Log(stateInfo.IsName(clip));
        return stateInfo.IsName(clip);
    }

    void Accelalation()
    {
        //現在の速度を取得
        v2 = rigid2D.velocity.y;

        //加速度を計算
        a = (v2 - v1) / Time.deltaTime;

        //現在の速度を、前のフレームの速度としてする
        v1 = v2;

    }


}

