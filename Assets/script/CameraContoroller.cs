using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContoroller : MonoBehaviour {

    GameObject player;
    Rigidbody2D rigid2D;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("player");
        rigid2D = player.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 playerPos = player.transform.position;
        float x = playerPos.x;
        float y = playerPos.y;
        
        if (playerPos.x < 0) x = 0;
        if (playerPos.y > 0) y = 0;
        //if(Mathf.Abs(rigid2D.velocity.y)<2)y=
        transform.position = new Vector3(x,y,transform.position.z);
	}
}
