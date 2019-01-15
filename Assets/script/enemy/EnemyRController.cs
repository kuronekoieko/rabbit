using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRController : MonoBehaviour
{


    string playerSide = "playerSide";

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //衝突を検知したのが"playerLR"ならprayerに攻撃
        if (other.gameObject.name.Equals(playerSide))
        {
            PlayerController.Hurt();
            //Debug.Log("R");
        }
    }


}
