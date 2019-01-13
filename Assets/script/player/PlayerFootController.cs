using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootController : MonoBehaviour
{

    public static string enemyHead = "enemyHead";

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //衝突を検知したのが"enemyHead"なら破壊する
        if (other.gameObject.tag.Equals(enemyHead)) { EnemyController.Death(other.gameObject.transform.parent.gameObject); }

    }




}
