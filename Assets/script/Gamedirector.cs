using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamedirector : MonoBehaviour
{


   static int starCount;
   static int HP;

   static int starCountMax = 7;
   static int starCountMin = 0;

   static int HPMax = 7;
   static int HPMin = 0;

    // Use this for initialization
    void Start()
    {
        InitializeStatus();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP == 0) PlayerDead();
    }

    public static void InitializeStatus() {

        HP = HPMax;
        starCount = starCountMin;
        Debug.Log("HP:" + HP);
        Debug.Log("star:" + starCount);
    }

    public static void StarCounter()
    {
        starCount++;
        starCount = Mathf.Clamp(starCount, starCountMin, starCountMax);
        Debug.Log("star:"+starCount);
    }

    public static void DecreaseHP()
    {
        HP--;
        HP = Mathf.Clamp(HP, HPMin, HPMax);
        Debug.Log("HP:" + HP);
    }

    public static void HealHP()
    {

        HP++;
        HP = Mathf.Clamp(HP, HPMin, HPMax);
        Debug.Log("HP:" + HP);
    }

    public static void PlayerDead() {
        // 現在のScene名を取得する
        Scene loadScene = SceneManager.GetActiveScene();
        // Sceneの読み直し
        SceneManager.LoadScene(loadScene.name);
        InitializeStatus();
    }

}
