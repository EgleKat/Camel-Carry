using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LevelController : MonoBehaviour {

    private bool levelStart;
    private float levelStartTime;
    public TextMeshProUGUI timerText;
    private float levelLimitTime;
    private GameObject camel;
    
    // Use this for initialization
    void Start()
    {
        levelLimitTime = 5;
        levelStart = false;
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
}
