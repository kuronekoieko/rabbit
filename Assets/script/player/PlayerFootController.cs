using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootController : MonoBehaviour
{

    public static string Player = "Player";
    public static string slugLR = "slugLR";
    public static string slugHead = "slugHead";
    public static string playerFoot = "playerFoot";


    void Start()
    {

    }
    
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //衝突を検知したのが"slugHead"なら死なす
        if (other.gameObject.tag.Equals(slugHead)) SlugController.Death();
    }




}
