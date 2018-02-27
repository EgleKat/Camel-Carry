using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Change_Main_Text : MonoBehaviour {

    private List<string> textMain = new List<string>();
    private List<string> textSecondary = new List<string>();

    public TextMeshProUGUI mainText;
    public TextMeshProUGUI secondaryText;

    public GameObject mainPanel;
    public GameObject secondaryPanel;

    private List<Vector3> secondaryPosition = new List<Vector3>();

    int textDisplayCount = 0;

    // Use this for initialization
    void Start () {
        textMain.Add("\nFinally! You're here!\n\n" +
            "Ohohoh you're in trouble my friend.\n\n" +
            "The Sultan increased the taxes and if you don't pay them you will go to jail!");
        textMain.Add("\nEvery day I will wait at the market for the goods and I will pay you good coin for your delivery services.\n\n" +
            "Your job is to load up my sweet camel G'Zilla efficiently so I receive enough goods and you receive your money.");
        textMain.Add("\nThe camel can carry up to 20 units of items and the heavier the load - the slower she goes");


        textSecondary.Add("These are all the items you have to deliver to me. Click on the item to place it in the chest.");
        textSecondary.Add("The red number represents the weight of the item and the yellow number - how much money you'll get for it.");
        textSecondary.Add("This is the chest's inventory.\nThe items here will be delivered on the next journey to the market. Below is the chest's weight. (Remember, the limit is 20)");
        textSecondary.Add("Here's the time until the market closes and your money goal for the day.");
        textSecondary.Add("Click on the camel when you're ready to deliver the goods. The day will start and you will be able to plan out your next shipment whilst the camel's travelling.");
        textSecondary.Add("And remember - you need to reach the goal before the market closes at the end of the day!\nGood Luck!");



        secondaryPosition.Add(new Vector3(-340, -108, -50));
        secondaryPosition.Add(new Vector3(-340, -108, -50));
        secondaryPosition.Add(new Vector3(-6, -90, -50));
        secondaryPosition.Add(new Vector3(110, -90, -50));
        secondaryPosition.Add(new Vector3(-75, 55, -50));
        secondaryPosition.Add(new Vector3(-75,55,-50));






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
            ChangeSecondaryText();
        }
        else
        {
            mainText.text = textMain[textDisplayCount];
            textDisplayCount++;
        }
    }

    public void ChangeSecondaryText()
    {

        Debug.Log(textDisplayCount + "  " + textSecondary.Count);
        if (textDisplayCount == textSecondary.Count)
        {
            secondaryPanel.SetActive(false);
        }else
        {
            secondaryPanel.transform.localPosition = secondaryPosition[textDisplayCount];
            secondaryText.text = textSecondary[textDisplayCount];
            textDisplayCount++;

        }
    }
}
