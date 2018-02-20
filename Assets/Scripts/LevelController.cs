using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LevelController : MonoBehaviour {

    private bool levelStart;
    private float levelStartTime;
    public TextMeshProUGUI timerText;
    public float levelLimitTime;
    private GameObject camel;
    public int coinGoal;
    private int coinCount = 0;
    public TextMeshProUGUI coinText;
    
    // Use this for initialization
    void Start()
    {
        levelStart = false;
        coinText.text = coinCount + " / " + coinGoal;
    }

    // Update is called once per frame
    void Update()
    {
        //Update timer
        if (levelStart)
        {
            float t = Time.time - levelStartTime;
            if (t >= levelLimitTime)
            {
                levelStart = false;
                timerText.text = "00:00";
                timerText.color = Color.red;
            }
            else
            {
                t = levelLimitTime - t;
                string minutes = Mathf.Floor(t / 60).ToString("00");
                string seconds = (t % 60).ToString("00");
               
            
                timerText.text = minutes + ":" + seconds;
            }

        }
    }

    public void StartTimer()
    {
        levelStart = true;
        levelStartTime = Time.time;
    }

    public void AddCoins(int newCoins)
    {
        coinCount += newCoins;
    }
}


