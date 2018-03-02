using System.Collections.Generic;
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
    private int level;

    public GameObject secondaryButton;
    private Boolean buttonActive;

    private List<Vector3> secondaryPosition = new List<Vector3>();
    private LevelController findlevel;

    InventoryManager controlClicking;

    int textDisplayCount = 0;

    // Use this for initialization
    void Start () {
        findlevel = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
        level = findlevel.GetLevel();
        
        //Used for enabling and disabling item swapping
        controlClicking = GameObject.FindGameObjectWithTag("inventory_manager").GetComponent<InventoryManager>();
        if (level == 1)
        {
            textMain.Add("\nFinally! You're here!\n\n" +
                "Ohohoh you're in trouble my friend.\n\n" +
                "The Sultan increased the taxes and if you don't pay them daily you will go to jail!");
            textMain.Add("\nDeliver the goods to me at the market and I will make sure you are paid well");
            //textMain.Add("\nEvery day I will wait at the market for the goods and I will pay you good coin for your delivery services.\n\n" +
            //    "Your job is to load up my sweet camel G'Zilla efficiently so I receive enough goods and you receive your money.");
            //textMain.Add("\nThe heavier the load - the slower my ol' G'Zilla goes");


            textSecondary.Add("Load the camel's chest by clicking on an item.");
            // textSecondary.Add("The red number represents the weight of the item and the yellow number - how much money you'll get for it.");
            //textSecondary.Add("Be careful. If the chest is too heavy, the camel won't move.");
            textSecondary.Add("This is your money goal for the day.");
            //textSecondary.Add("And remember - you need to reach the goal before the market closes!\nGood Luck!");
            textSecondary.Add("Click on the camel when you're ready to deliver the goods.");



            secondaryPosition.Add(new Vector3(-340, -108, -50));
            //  secondaryPosition.Add(new Vector3(-340, -108, -50));
            //secondaryPosition.Add(new Vector3(-6, -90, -50));
            secondaryPosition.Add(new Vector3(110, -90, -50));
            //secondaryPosition.Add(new Vector3(-75, 55, -50));
            secondaryPosition.Add(new Vector3(-75, 55, -50));



            buttonActive = false;


            mainPanel.SetActive(true);
            secondaryPanel.SetActive(false);
            ChangeMainText();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void ChangeMainText()
    {
        if (level == 1)
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
    }

    //find border scripts
    public FlashBorder GetBorder(string name)
    {
        return GameObject.Find(name).GetComponent<FlashBorder>();
    }

    public void ChangeSecondaryText()
    {
        switch (level)
        {
            case 1:
                switch (textDisplayCount)
                {
                    //click item in inventory
                    case 0:
                        GetBorder("InventoryBorder").Highlight();
                        break;
                    //money
                    case 1:
                        GetBorder("InventoryBorder").Hide();
                        GetBorder("InfoOutline").Highlight();
                        break;
                    case 2:
                        GetBorder("InfoOutline").Hide();
                        //click the camel and go!!!
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
                break;
            case 2:
                controlClicking.SetCamelClickable(true);
                break;

        }
        
    }
}
