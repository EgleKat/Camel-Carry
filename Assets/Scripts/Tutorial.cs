﻿using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Tutorial : MonoBehaviour {

    private List<string> textMain = new List<string>();
    private List<string> textSecondary = new List<string>();

    public TextMeshProUGUI mainText;
    public TextMeshProUGUI secondaryText;

    public GameObject mainPanel;
    public GameObject secondaryPanel;

    public GameObject secondaryButton;
    private Boolean buttonActive;

    private List<Vector3> secondaryPosition = new List<Vector3>();

    InventoryManager controlClicking;

    int textDisplayCount = 0;

    // Use this for initialization
    void Start () {
        //Used for enabling and disabling item swapping
        controlClicking = GameObject.FindGameObjectWithTag("inventory_manager").GetComponent<InventoryManager>();

        textMain.Add("\nFinally! You're here!\n\n" +
            "Ohohoh you're in trouble my friend.\n\n" +
            "The Sultan increased the taxes and if you don't pay them you will go to jail!");
        textMain.Add("\nEvery day I will wait at the market for the goods and I will pay you good coin for your delivery services.\n\n" +
            "Your job is to load up my sweet camel G'Zilla efficiently so I receive enough goods and you receive your money.");
        textMain.Add("\nThe heavier the load - the slower my ol' G'Zilla goes");


        textSecondary.Add("Load the camel's chest by clicking on an item.");
       // textSecondary.Add("The red number represents the weight of the item and the yellow number - how much money you'll get for it.");
        textSecondary.Add("Be careful. If the chest is too heavy, the camel won't move.");
        textSecondary.Add("Here's the time until the market closes and your money goal for the day.");
        textSecondary.Add("And remember - you need to reach the goal before the market closes!\nGood Luck!");
        textSecondary.Add("Click on the camel when you're ready to deliver the goods. The day will start and you will be able to plan out your next shipment whilst the camel's travelling.");



        secondaryPosition.Add(new Vector3(-340, -108, -50));
      //  secondaryPosition.Add(new Vector3(-340, -108, -50));
        secondaryPosition.Add(new Vector3(-6, -90, -50));
        secondaryPosition.Add(new Vector3(110, -90, -50));
        secondaryPosition.Add(new Vector3(-75, 55, -50));
        secondaryPosition.Add(new Vector3(-75,55,-50));



        buttonActive = false;


        mainPanel.SetActive(true);
        secondaryPanel.SetActive(false);
        ChangeMainText();

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void ChangeMainText()
    {
        if (textDisplayCount == textMain.Count)
        {
            mainPanel.SetActive(false);
            secondaryPanel.SetActive(true);
            textDisplayCount = 0;
            controlClicking.SetSwap(true);
            ChangeSecondaryText();
        }
        else
        {
            mainText.text = textMain[textDisplayCount];
            textDisplayCount++;
            controlClicking.SetSwap(false);
           
        }
    }

    //find border scripts
    public FlashBorder GetBorder(string name)
    {
        return GameObject.Find(name).GetComponent<FlashBorder>();
    }

    public void ChangeSecondaryText()
    {

        switch (textDisplayCount)
        {
            //click item in inventory
            case 0:
                GetBorder("InventoryBorder").Highlight();
                break;
            //careful of weight
            case 1:
                GetBorder("InventoryBorder").Hide();
                GetBorder("MaxWeightBorder").Highlight();
                break;
            //time and money
            case 2:
                GetBorder("MaxWeightBorder").Hide();
                GetBorder("InfoOutline").Highlight();
                break;
            case 3:
                GetBorder("InfoOutline").Hide();
                break;
            //click the camel and go!!!
            case 4:
                controlClicking.SetTutorialFinished(true);

                if (!(controlClicking.GetNumItemsInCamelInventory() == 0))
                {
                    controlClicking.SetCamelClickable(true);
                }

                buttonActive = false;
                break;
            //tutorial over
            default:
                secondaryPanel.SetActive(false);
                return;
        }

        secondaryPanel.transform.localPosition = secondaryPosition[textDisplayCount];
        secondaryText.text = textSecondary[textDisplayCount];
        textDisplayCount++;
        secondaryButton.SetActive(buttonActive);
        buttonActive = true;
    }
}
