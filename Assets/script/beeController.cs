using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beeController : MonoBehaviour {

    Rigidbody2D rigid2D;
    Animator animator;



    void Start () {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rigid2D.gravityScale = 0;
    }
	
	
	void Update () {
        rigid2D.velocity = new Vector3(0,0,0);
    }
}
