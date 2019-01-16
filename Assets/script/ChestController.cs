using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChestController : MonoBehaviour
{


    public Sprite[] sprites;
    public GameObject star;
    public static int starNum;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.name.Equals("player") && GetComponent<SpriteRenderer>().sprite == sprites[0])
        {
            StartCoroutine(DelayMethod(0.3f, () =>
            {
                GameObject go = Instantiate(star, gameObject.transform.position, Quaternion.identity);
                starNum++;
                go.name = "star (" + starNum + ")";
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
