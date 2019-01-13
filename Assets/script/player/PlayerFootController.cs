using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootController : MonoBehaviour
{

    public static string slugHead = "slugHead";

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //衝突を検知したのが"slugHead"なら破壊する
        if (other.gameObject.tag.Equals(slugHead)) { SlugController.Death(other.gameObject.transform.parent.gameObject); }

    }




}
