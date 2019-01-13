using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLController : MonoBehaviour
{

    string playerSide = "playerSide";

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //衝突を検知したのが"playerLR"ならprayerに攻撃
        if (other.gameObject.tag.Equals(playerSide))
        {
            PlayerController.Hurt();
            //Debug.Log("L");
        }
    }



}
