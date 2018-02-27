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
    private GameObject winMessage;
    private GameObject looseMessage;
    private InventoryManager inventoryManager;
    private AudioSource tickAudio;
    private float time;
    private bool onFiveSecs=false;

    // Use this for initialization
    void Start()
    {
        levelStart = false;
        setCoinDisplay();

        tickAudio = timerText.GetComponent<AudioSource>();

        inventoryManager = GameObject.FindGameObjectWithTag("inventory_manager").GetComponent<InventoryManager>();
        camel = GameObject.FindGameObjectWithTag("Camel");
        winMessage = GameObject.FindWithTag("win_message");
        winMessage.SetActive(false);

        looseMessage = GameObject.FindWithTag("loose_message");
        looseMessage.SetActive(false);

        float t = levelLimitTime;
        string minutes = Mathf.Floor(t / 60).ToString("00");
        string seconds = (t % 60).ToString("00");
        timerText.text = minutes + ":" + seconds;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Update timer
        if (levelStart)
        {
            
            float t = Time.time - levelStartTime;
            time = levelLimitTime - t;


            if (time < 6 && !onFiveSecs)
            {
                onFiveSecs = true;
                timerText.color = Color.red;
                InvokeRepeating("playTickSound", 0, 1);
            }

            //Adjust time text
                string minutes = Mathf.Floor(time / 60f).ToString("00");
                string seconds = Mathf.Floor(time % 60f).ToString("00");
                           
                timerText.text = minutes + ":" + seconds;

            if (time <=0)
            {
                levelStart = false;
                LevelFinished();
                timerText.text = "00:00";
                
            }
        }
    
    }

    private void playTickSound()
    {

        if(time > 0)
        {
            tickAudio.Stop();
            tickAudio.Play();
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
    //called either when time is finished or when all the items are sold
    public void LevelFinished()
    {
        Moving camelMoving = camel.GetComponent<Moving>();
        //Don't let the user click the camel and stop the camel from moving
        camelMoving.SetClickable(false);
        camelMoving.StopMoving(false);
        camelMoving.TurnAllMusicOff();

        //Inventory is uninteractable when level finished
        inventoryManager.SetSwap(false);

        //check if coin goal is met
        if (coinCount>=coinGoal)
        {
            DisplayWinMessage();
        }
        else
        {
            DisplayLooseMessage();
        }

    }

    private void DisplayLooseMessage()
    {
        looseMessage.SetActive(true);
        
    }

    private void DisplayWinMessage()
    {
        winMessage.SetActive(true);

    }
}


