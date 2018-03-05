using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LevelController : MonoBehaviour
{

    private bool levelStart;
    private float levelStartTime;
    private TextMeshProUGUI timerText;
    public float levelLimitTime;
    private GameObject camel;
    public int coinGoal;
    private int coinCount = 0;
    private TextMeshProUGUI coinText;
    private int weight;
    private GameObject winMessage;
    private GameObject looseMessage;
    private InventoryManager inventoryManager;
    private AudioSource tickAudio;
    private UserControlls userControlls;
    private float time;
    private bool onFiveSecs = false;
    private bool backOnFiveSecs = true;
    private Slider speedSlider;

    public int level;
    private bool finished;
    public bool stopTime;
    private float timeSpentStanding;

    // Use this for initialization
    void Start()
    {
        userControlls = GameObject.Find("UserController").GetComponent<UserControlls>();
        finished = false;
        timeSpentStanding = 0;
        //Enable timer
        if (level > 1)
        {
            timerText = GameObject.FindGameObjectWithTag("theTimer").GetComponent<TextMeshProUGUI>();
            tickAudio = timerText.GetComponent<AudioSource>();
        }
        //Enable speed slider
        if (level > 2)
        {
            speedSlider = GameObject.FindGameObjectWithTag("Time_Slider").GetComponent<Slider>();

        }
        coinText = GameObject.Find("Nmber of Coins").GetComponent<TextMeshProUGUI>();
        levelStart = false;
        setCoinDisplay();



        inventoryManager = GameObject.FindGameObjectWithTag("inventory_manager").GetComponent<InventoryManager>();
        camel = GameObject.FindGameObjectWithTag("Camel");
        winMessage = GameObject.FindWithTag("win_message");
        winMessage.SetActive(false);

        looseMessage = GameObject.FindWithTag("loose_message");
        looseMessage.SetActive(false);

        if (!(level == 1))
        {

            float t = levelLimitTime;
            string minutes = Mathf.Floor(t / 60).ToString("00");
            string seconds = (t % 60).ToString("00");
            timerText.text = minutes + ":" + seconds;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!(level == 1) && finished == false)
        {
            //Update timer
            if (levelStart && !stopTime)
            {

                float t = Time.time - timeSpentStanding - levelStartTime;
                time = levelLimitTime - t;


                if (time < 6 && !onFiveSecs)
                {
                    onFiveSecs = true;
                    backOnFiveSecs = true;
                    timerText.color = Color.red;
                    InvokeRepeating("playTickSound", 0, 1);
                }

                //Adjust time text
                string minutes = Mathf.Floor(time / 60f).ToString("00");
                string seconds = Mathf.Floor(time % 60f).ToString("00");

                timerText.text = minutes + ":" + seconds;

                if (time <= 0)
                {
                    levelStart = false;
                    LevelFinished();
                    timerText.text = "00:00";

                }
            }
            //Count how much time has passed in every frame the camel's been standing
            else if (stopTime)
            {
                timeSpentStanding += Time.deltaTime;
                if (backOnFiveSecs)
                {
                    CancelInvoke();
                    onFiveSecs = false;
                    backOnFiveSecs = false;
                }
            }
        }
    }

    private void playTickSound()
    {

        if (time > 0)
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
        finished = true;
        //check if coin goal is met
        if (coinCount >= coinGoal)
        {
            DisplayWinMessage();
        }
        else
        {
            DisplayLooseMessage();
        }
        userControlls.SetInstructedToRestart(false);
        
        if (level > 2)
        {
            //Turn off speed slider, turn back time to 1
            speedSlider.value = 1;
            speedSlider.interactable = false;
        }
        Moving camelMoving = camel.GetComponent<Moving>();
        //Don't let the user click the camel and stop the camel from moving
        camelMoving.SetClickable(false);
        camelMoving.StopMoving(false);
        camelMoving.TurnAllMusicOff();
        CancelInvoke();

        //Inventory is uninteractable when level finished
        inventoryManager.SetSwap(false);

    }

    private void DisplayLooseMessage()
    {
        looseMessage.SetActive(true);

    }

    private void DisplayWinMessage()
    {
        winMessage.SetActive(true);

    }

    public void SetLevel(int lvl)
    {
        level = lvl;
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetCoinCount()
    {
        return coinCount;
    }

    public int GetCoinGoal()
    {
        return coinGoal;
    }

}


