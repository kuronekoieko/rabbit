using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContoroller : MonoBehaviour
{

    GameObject player;
    Rigidbody2D rigid2D;
    float x;
    float y1;//１フレーム前のy座標
    float y2;//現在のy座標
    float delta_y = 0.1f;
    Vector3 playerPos;


    void Start()
    {
        player = GameObject.Find("player");
        rigid2D = player.GetComponent<Rigidbody2D>();
        playerPos = player.transform.position;
        y1 = playerPos.y;
    }

    void Update()
    {
        playerPos = player.transform.position;
        x = playerPos.x;
        y2 = playerPos.y + 1.0f;

        //前のフレームとのy座標の差がごくわずかならば、カメラ移動しない
        if (Mathf.Abs(y2 - y1) < delta_y && PlayerController.isGrounded) y2 = y1;

        x = Mathf.Clamp(x, 0.0f, x);
        y2 = Mathf.Clamp(y2, y2, 0.0f);
        transform.position = new Vector3(x, y2, transform.position.z);

        y1 = y2;
    }
}
