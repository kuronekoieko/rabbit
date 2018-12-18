using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugLRController : MonoBehaviour {

    string playerFoot = "playerFoot";
    string player = "player";

    void Start () {
		
	}
	
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //衝突を検知したのが"playerFoot"なら死ぬ
        //if (other.gameObject.tag.Equals(playerFoot)) slugController.Death();
        if (other.gameObject.tag.Equals(player)) Debug.Log("slugLR");
    }



}
