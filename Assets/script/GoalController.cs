using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    public GameObject clearTextObj;
    bool isCleard;

    // Use this for initialization
    void Start()
    {
        clearTextObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (isCleard) return;

        //衝突を検知したのが"playerLR"ならprayerに攻撃
        if (other.gameObject.name.Equals("player"))
        {
            isCleard = true;
            clearTextObj.SetActive(true);
            //数秒後に消す
            StartCoroutine(DelayMethod(3.0f, () =>
            {
                SceneManager.LoadScene("StartScene");

            }));

        }
    }

    public static IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

}
