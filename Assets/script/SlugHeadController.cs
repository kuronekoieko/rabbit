using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugHeadController : MonoBehaviour {

    string playerFoot = "playerFoot";
    SlugController slugController;


    void Start () {
        slugController = new SlugController();
    }
	
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //衝突を検知したのが"playerFoot"なら死ぬ
        //if (other.gameObject.tag.Equals(playerFoot)) slugController.Death();
    }




}
