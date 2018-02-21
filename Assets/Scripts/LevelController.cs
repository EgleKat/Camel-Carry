using System;
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
    private int weight;
    
    // Use this for initialization
    void Start()
    {
        levelStart = false;
        setCoinDisplay();

        float t = levelLimitTime;
        string minutes = Mathf.Floor(t / 60).ToString("00");
        string seconds = (t % 60).ToString("00");
        timerText.text = minutes + ":" + seconds;
    }

    // Update is called once per frame
    void Update()
    {
        //Update timer
        if (levelStart)
        {
            float t = Time.time - levelStartTime;
            
           
            if (t > levelLimitTime - 6)
            {
                timerText.faceColor = Color.red;
            }

            //Adjust time text
            float time = levelLimitTime - t;
                string minutes = Mathf.Floor(time / 60f).ToString("00");
                string seconds = Mathf.Floor(time % 60f).ToString("00");
                           
                timerText.text = minutes + ":" + seconds;

            if (t >= levelLimitTime)
            {
                levelStart = false;
                timerText.text = "00:00";
                
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
        setCoinDisplay();
    }

    private void setCoinDisplay()
    {
        coinText.text = coinCount + " / " + coinGoal;
    }

    public void SetWeight(int weight)
    { 
        this.weight = weight;
    }
    public int GetWeight()
    {
        return this.weight;
    }
}


