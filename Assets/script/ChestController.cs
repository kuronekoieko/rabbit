using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChestController : MonoBehaviour {


    public Sprite[] sprites;
    public GameObject star;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.name.Equals("player")&& GetComponent<SpriteRenderer>().sprite == sprites[0]) {
            float x = gameObject.transform.position.x;
            float y = gameObject.transform.position.y;
            float z = gameObject.transform.position.z;
            StartCoroutine(DelayMethod(0.3f, () =>
            {
                Instantiate(star, new Vector3(x, y, z), Quaternion.identity);
            }));

            GetComponent<SpriteRenderer>().sprite = sprites[1];

        }
    }

    public static IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
