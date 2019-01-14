using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    GetComponent<Rigidbody2D>().velocity
                    = new Vector2(3.0f * -1, 0);

        Destroy(gameObject, 3.0f);
    }


    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.name.Equals("player")) PlayerController.Hurt();
    }
}
