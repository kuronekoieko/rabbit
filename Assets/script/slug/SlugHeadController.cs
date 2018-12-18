using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugHeadController : MonoBehaviour {

    string playerFoot = "playerFoot";

    string slug = "slug";
    string slugLR = "slugLR";
    string slugHead = "slugHead";
    bool isCollisionSlug;
    float jumpYForce = 900.0f;



    void Start () {
    }
	
	void Update () {
		
	}


    void OnTriggerEnter2D(Collider2D other)
    {

        //slugに衝突したときの動作
        if (other.gameObject.tag.Equals(playerFoot))
        {
            //CollisionSlug(other.gameObject.AddComponent<Rigidbody2D>());
            //Debug.Log(slugHead);
            PlayerController.CollisionSlug();

        }

    }

    public void CollisionSlug(Rigidbody2D rigid2D)
    {
        //Debug.Log(slugHead);
        if (isCollisionSlug) return;

        isCollisionSlug = true;
        rigid2D.AddForce(transform.up * jumpYForce * 2.0f);
        PlayerAmination.JumpAnim();
    }



}
