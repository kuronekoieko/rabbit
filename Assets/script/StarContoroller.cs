using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarContoroller : MonoBehaviour
{

    GameObject mGo;

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (!mGo) return;
            if (mGo == gameObject) {
                Gamedirector.StarCounter();
                Destroy(gameObject);
            }

        }
    }

   public void SetGameObject(GameObject go)
    {
        mGo = go;

    }
}
