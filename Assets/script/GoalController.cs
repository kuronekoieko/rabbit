using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalController : MonoBehaviour {


   public GameObject clearTextObj;

    // Use this for initialization
    void Start () {
       //clearTextObj = transform.Find("Canvas/clearText").gameObject;
        clearTextObj.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //衝突を検知したのが"playerLR"ならprayerに攻撃
        if (other.gameObject.name.Equals("player"))
        {
            clearTextObj.SetActive(true);
            Debug.Log("Trigger");
        }
    }

}
