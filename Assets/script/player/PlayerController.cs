using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{

    public static Rigidbody2D rigid2D;
    public static float jumpYForce = 20.0f;
    public static float walkSpeed = 10.0f;
    public static Vector3 buttonDownPosition;
    public static Vector3 buttonPosition;
    public static Vector3 buttonRereasePosition;
    public static Vector3 flickVector;
    public static int key;
    public static Animator animator;
    public static float nearyZero = 0.1f;
    public static bool isMouseRerease = true;
    public static Vector2 playerPosition;
    public static bool isLaddering;
    public static float ladderingSpeed = 3.0f;
    public static int keyX;
    public static int keyY;
    public static bool isCollisionEnemy;
    public static float tappingTime;
    public static bool isHurting;
    protected static float defaultGravityScale;
    public static float sin;
    public static GameObject collisionEnemyObj;
    public static float vyMax = 20.0f;
    [SerializeField] public Transform groundCheck_L;
    [SerializeField] public Transform groundCheck_C;
    [SerializeField] public Transform groundCheck_R;
    public static bool isGrounded;

    public static string ladder = "ladder";
    public static string enemy = "enemy";
    public static string Tilemap = "Tilemap";
    public static string enemyHead = "enemyHead";

    //フレーム毎の処理====================================================================================================

    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        defaultGravityScale = rigid2D.gravityScale;
    }

    void Update()
    {
        //接地判定
        GroundCheck();

        //アニメーションの切り替え
        SwhichAnimation();

        //フリック時の動作
        if (isLaddering)
        {
            //はしごを登ってるとき
            FlickLaddering();
        }
        else
        {
            //通常時
            if (!isHurting) Flick();
        }

        //落ちたら戻る
        if (transform.position.y < -50) Gamedirector.PlayerDead();

        //スピード制限
        //SpeedLimitter();
    }

    //コライダが呼ばれたときの処理========================================================================================================


    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag.Equals(ladder))
        {
            //はしごに入った瞬間に呼ばれる
            isLaddering = true;
            rigid2D.gravityScale = 0;
        }

        //あたったスラッグのオブジェクトを取得
        if (other.gameObject.name.Equals("headCollider")) collisionEnemyObj = other.gameObject;


    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals(ladder))
        {
            //はしごを抜けた瞬間に呼ばれる
            rigid2D.gravityScale = defaultGravityScale;
            isLaddering = false;
        }

        if (other.gameObject.tag.Equals(enemyHead))
        {
            //enemyから離れたときの処理
            isCollisionEnemy &= !other.gameObject.tag.Equals(enemyHead);
        }

    }

    void OnCollisionStay2D(Collision2D other)
    {
    }

    void OnCollisionExit2D(Collision2D collision)
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Tilemapに衝突したとき宝箱に乗ったときに、ダメージを解除する
        if (collision.gameObject.name.Equals(Tilemap) || collision.gameObject.tag.Equals("chest"))
        {
            isHurting = false;
        }
    }

    //普通のメソッド==============================================================================================================

    public static void FlickLaddering()
    {
        animator.speed = keyX == 0 && keyY == 0 ? 0f : 1f;

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
            if (flickVector.x > 0) keyX = 1;
            if (flickVector.x < 0) keyX = -1;

            //y方向の向きを見て、方向を判定する
            if (flickVector.y > 0) keyY = 1;
            if (flickVector.y < 0) keyY = -1;

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

    public static void CollisionEnemy(GameObject enemyObj)
    {
        if (!enemyObj == collisionEnemyObj) return;
        if (isCollisionEnemy) return;
        isHurting = false;
        isCollisionEnemy = true;
        rigid2D.velocity = new Vector2(rigid2D.velocity.x, jumpYForce);
        PlayerAmination.JumpAnim();
    }

    public static void Hurt()
    {
        if (isHurting) return;
        isHurting = true;
        rigid2D.velocity = new Vector3(0, 0, 0);
        rigid2D.velocity = new Vector3(3.0f * -key, jumpYForce, 0);
        Gamedirector.DecreaseHP();
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
        tappingTime += Time.deltaTime;

        //x方向の向きを見て、方向を判定する
        key = 0;
        if (flickVector.x > 0) key = 1;
        if (flickVector.x < 0) key = -1;

        //移動
        Skip();
    }

    public static void FlickEnd()
    {
        //タップを離したときのジャンプ
        Jump();

        //以下初期化
        isMouseRerease = true;
        key = 0;
        buttonPosition.Set(0, 0, 0);
        buttonDownPosition.Set(0, 0, 0);
        rigid2D.velocity = new Vector2(0, rigid2D.velocity.y);
        tappingTime = 0;
    }

    public static void Jump()
    {
        buttonRereasePosition = Input.mousePosition;
        Vector3 tappingVector = buttonRereasePosition - buttonDownPosition;

        float distance2 = Mathf.Pow(tappingVector.x, 2) + Mathf.Pow(tappingVector.y, 2) + Mathf.Pow(tappingVector.z, 2);
        float distance = Mathf.Sqrt(distance2);

        bool isTap = Mathf.Abs(distance) < 1.0f && tappingTime < 0.2f;

        if (isTap && isGrounded && !isLaddering)
        {
            rigid2D.velocity = new Vector2(rigid2D.velocity.x, jumpYForce);
        }
    }


    public static void Skip()
    {
        if (isHurting) return;

        //歩いてるときは等速で移動
        rigid2D.velocity = new Vector2(walkSpeed * key, rigid2D.velocity.y);
        float x = Mathf.Abs(rigid2D.transform.localScale.x) * key;
        float y = rigid2D.transform.localScale.y;
        if (key != 0) rigid2D.transform.localScale = new Vector2(x, y);

    }

    private void GroundCheck()
    {
        //初期化
        isGrounded = false;

        //L、C、Rそれぞれが格納できる配列を定義
        Collider2D[][] groundCheckCollider = new Collider2D[3][];

        //L、C、Rそれぞれの検知したコライダの配列を格納
        groundCheckCollider[0] = Physics2D.OverlapPointAll(groundCheck_L.position);
        groundCheckCollider[1] = Physics2D.OverlapPointAll(groundCheck_C.position);
        groundCheckCollider[2] = Physics2D.OverlapPointAll(groundCheck_R.position);

        //groundCheckをL,C,Rで回す
        foreach (Collider2D[] groundColliderList in groundCheckCollider)
        {
            //それぞれが検知したコライダを回す
            foreach (Collider2D groundCollider in groundColliderList)
            {
                //nullチェック
                if (!groundCollider) break;
                //Tilemapを検知したら接地
                if (groundCollider.gameObject.name.Equals(Tilemap)) isGrounded = true;
                //宝箱を検知したら接地
                if (groundCollider.gameObject.tag.Equals("chest")) isGrounded = true;
            }
        }
    }

    private void SwhichAnimation()
    {
        //ジャンプアニメーション
        if (rigid2D.velocity.y > 0.0f && !isGrounded && !isHurting && !isLaddering) PlayerAmination.JumpAnim();

        //落下時アニメーション
        if (rigid2D.velocity.y < -2 && !isGrounded && !isHurting && !isLaddering) PlayerAmination.FallAnim();

        //攻撃をうけたときのアニメーション
        if (isHurting && !isGrounded) PlayerAmination.HurtAnim();

        //idleアニメーション
        if (key == 0 && isGrounded) PlayerAmination.IdleAnim();

        //スキップアニメーション
        if (!isLaddering && key != 0 && isGrounded) PlayerAmination.SkipAnim();

        //はしごアニメーション
        if (isLaddering) PlayerAmination.ClimbAnim();

    }

    private static void SpeedLimitter()
    {
        //y方向の速度制限
        float vy = Mathf.Clamp(rigid2D.velocity.y, rigid2D.velocity.y, vyMax);
        rigid2D.velocity = new Vector2(rigid2D.velocity.x, vy);
    }

}

