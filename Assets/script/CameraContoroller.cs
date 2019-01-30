using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContoroller : MonoBehaviour
{

    public GameObject player;
    float camera_x;//カメラのx座標
    float camera_y;//カメラのy座標
    float camera_x_min=0f;//カメラのx座標
    float camera_y_min;//カメラのy座標
    float camera_x_max= 92.0f-9.0f;//カメラのx座標
    float camera_y_max=0f;//カメラのy座標
    float range_y = 2.0f;//プレイヤーをカメラに収める範囲
    float range_x = 1.5f;//プレイヤーをカメラに収める範囲
    Vector3 playerPos;


    void Start()
    {
    }

    void Update()
    {
        //プレイヤーの座標を取得
        playerPos = player.transform.position;

        //カメラの座標を設定
        camera_x = transform.position.x;
        camera_y = transform.position.y;

        //キャラが内枠を出たときに、カメラのx座標を移動する
        if (playerPos.x - camera_x > range_x)
        {
            camera_x = playerPos.x - range_x;
        }
        else if (playerPos.x - camera_x < -range_x)
        {
            camera_x = playerPos.x + range_x;
        }

        //キャラが内枠を出たときに、カメラのy座標を移動する
        if (playerPos.y - camera_y > range_y)
        {
            camera_y = playerPos.y - range_y;
        }
        else if (playerPos.y - camera_y < -range_y)
        {
            camera_y = playerPos.y + range_y;
        }

        //カメラの移動範囲制限
        camera_x = Mathf.Clamp(camera_x, 0.0f, camera_x_max);
        camera_y = Mathf.Clamp(camera_y, camera_y, 0.0f);

        //移動処理
        transform.position = new Vector3(camera_x, camera_y, transform.position.z);
    }
}
