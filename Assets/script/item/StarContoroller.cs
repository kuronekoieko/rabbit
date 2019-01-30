using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarContoroller : MonoBehaviour
{


    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 10.0f);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("itemCheck"))
        {
            Gamedirector.StarCounter();
            Destroy(gameObject);
        }
    }
}
