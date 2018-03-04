using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{

    public List<GameObject> inventory;
    public List<GameObject> camelInventory;

    public GameObject placeholder;

    AudioSource sellItems;
    AudioSource error;

    public int maxWeight;
    public int numItemsInInventory;
    private bool canSwap;

    private int currentCamelInventoryWeight;
    private int numberItemsInCamelInventory;
    private TextMeshProUGUI weightText;

    private Moving camelMoving;
    private LevelController levelController;
    private Tutorial tutorialScript;

    //for tutorial
    private bool firstItemMoved;
    private bool tutorialFinished;

    // Use this for initialization
    void Start()
    {
        firstItemMoved = false;
        tutorialFinished = false;
        sellItems = GetComponents<AudioSource>()[0];
        error = GetComponents<AudioSource>()[1];
        tutorialScript = GameObject.Find("Tutorial").GetComponent<Tutorial>();

        numberItemsInCamelInventory = 0;

        levelController = GameObject.FindWithTag("GameController").GetComponent<LevelController>();

        foreach (GameObject item in inventory)
        {
            ItemValues itemVals = item.GetComponent<ItemValues>();

            //customisable item and weight for each item
            //items are organised into types: normal, cold and hot
            //for multiple items change multiplier
            switch (item.name)
            {
                case "Umbrella":
                    itemVals.SetPrice(20);
                    itemVals.SetWeight(8);
                    goto case "Normal";
                case "Sword":
                    itemVals.SetPrice(100);
                    itemVals.SetWeight(10);
                    if (levelController.GetLevel() == 2)
                    {
                        itemVals.SetPrice(200);
                        itemVals.SetWeight(18);
                    }
                    goto case "Normal";
                case "Flip-Flops":
                    itemVals.SetPrice(10);
                    itemVals.SetWeight(2);
                    goto case "Normal";
                case "Mysterious-bottle":
                    itemVals.SetPrice(30);
                    itemVals.SetWeight(1);
                    goto case "Normal";
                case "Normal":
                    itemVals.SetType(ItemValues.ItemType.Normal);
                    break;

                case "Ice Cream":
                    itemVals.SetPrice(50);
                    itemVals.SetWeight(5);
                    goto case "Cold";
                case "Ice Cube":
                    itemVals.SetPrice(30);
                    itemVals.SetWeight(12);
                    goto case "Cold";
                case "Cold":
                    itemVals.SetType(ItemValues.ItemType.Cold);
                    break;

                case "Microwave":
                    itemVals.SetPrice(50);
                    itemVals.SetWeight(5);
                    goto case "Hot";
                case "Hot":
                    itemVals.SetType(ItemValues.ItemType.Hot);
                    break;

                default:
                    Debug.Log("Item Not Recognised");
                    break;
            }

        }

        if (!(levelController.GetLevel() == 1))
        {
            weightText = GameObject.Find("MaxWeight").GetComponent<TextMeshProUGUI>();
            weightText.text = "0";
        }

        currentCamelInventoryWeight = 0;
		camelMoving = GameObject.FindGameObjectWithTag ("Camel").GetComponent<Moving> ();

        levelController.SetWeight(0);   //set the weight to 0
		canSwap = true;
    }

    // Update is called once per frame
    void Update()
    {

    }


	// Swap positions of two game objects
	private void SwapPos (GameObject a, GameObject b)
	{
		Vector3 tempPosition = a.transform.position;
		a.transform.position = b.transform.position;
		b.transform.position = tempPosition;

	}
    
    // Removes item from inventory and puts it in camel
    private void MoveItemToCamel (GameObject objectToMove)
    {
		GameObject objectToMove2 = null;

        //pass first part of tutorial
        if (!firstItemMoved && levelController.GetLevel() < 3)
            tutorialScript.ChangeSecondaryText();

        firstItemMoved = true;

        int objectWeight = objectToMove.GetComponent<ItemValues>().GetWeight();

		//find placeholder to swap with
		foreach (GameObject item in camelInventory) {
			if (item.tag == "placeholder") {
				objectToMove2 = item;
				break;
			}
		}


        bool isObjectToMoveCompatable = true;
        foreach (GameObject item in camelInventory)
        {
            if (item.tag == "placeholder")
                continue;

            if (ItemsCompatable(objectToMove, item)) 
                continue;

            isObjectToMoveCompatable = false;
            break;

        }

        bool shouldReturn = false;
        //if no placeholder or item is uncompatable dont add item
        if (objectToMove2 == null || !isObjectToMoveCompatable)
        {
            //flash inventory if too many items or items not compatible
            StartCoroutine(GameObject.Find("CamelInventoryBorder").GetComponent<FlashBorder>().FlashBorderRed());
            shouldReturn = true;
        }
        //if object would break the camels back don't allow adding
        if (currentCamelInventoryWeight + objectWeight > maxWeight)
        {
            //flash max weight red
            StartCoroutine(GameObject.Find("MaxWeightBorder").GetComponent<FlashBorder>().FlashBorderRed());
            shouldReturn = true;
        }

        if (shouldReturn)
        {
            error.Play();
            return;
        }
        //get location of each object
        int object2Index = camelInventory.IndexOf (objectToMove2);
		int object1Index = inventory.IndexOf (objectToMove);

        //swap inventories of items
		inventory[object1Index] = objectToMove2;
		camelInventory[object2Index] = objectToMove;

		SwapPos (objectToMove, objectToMove2);

        //update camel weight
        currentCamelInventoryWeight += objectToMove.GetComponent<ItemValues>().GetWeight();
        levelController.SetWeight(currentCamelInventoryWeight);

        if (!(levelController.GetLevel() == 1))
        {
            weightText.text = currentCamelInventoryWeight.ToString() + "/20";
        }

        //Item added, now camel can go to market as long as tutorial is finished
        if(numberItemsInCamelInventory == 0 && tutorialFinished || levelController.GetLevel() > 2)
        {
            camelMoving.SetClickable(true);
        }
        numberItemsInCamelInventory++;
        numItemsInInventory--;
    }

    // Removes item from camel and puts it in inventory
    private void MoveItemToInventory(GameObject objectToMove)
    {
		GameObject objectToMove2 = null;

		//find placeholder to swap with
		foreach (GameObject item in inventory) {
			if (item.tag == "placeholder") {
				objectToMove2 = item;
				break;
			}
		}

		//if no placeholder dont add item
		if (objectToMove2 == null)
			return;

		//move items
		int object2Index = inventory.IndexOf (objectToMove2);
		int object1Index = camelInventory.IndexOf (objectToMove);

		inventory[object2Index] = objectToMove;
		camelInventory[object1Index] = objectToMove2;

		SwapPos (objectToMove, objectToMove2);

        //update camel weight
        currentCamelInventoryWeight -= objectToMove.GetComponent<ItemValues>().GetWeight();
        levelController.SetWeight(currentCamelInventoryWeight);


        if (!(levelController.GetLevel() == 1))
        {
            weightText.text = currentCamelInventoryWeight.ToString() + "/20";
        }

        //can't send camel to market with no items
        if(numberItemsInCamelInventory == 1)
        {
            camelMoving.SetClickable(false);
        }

        numberItemsInCamelInventory--;
        numItemsInInventory++;

    }


    public void ToggleInventory (GameObject objectToMove)
    {
        //check if items can be exchanged between invenotries
		if (!canSwap)
			return;
		
        //check where item is and move to other inventory
        if (inventory.Contains (objectToMove))
        {
            MoveItemToCamel (objectToMove);
        }
        else
        {
            MoveItemToInventory (objectToMove);
        }
    }

	public IEnumerator SellItems ()
	{
       
		int totalItemValue = 0;

        sellItems.Play();
        //loop through all items in camels inventory
        for (int i = 0; i < camelInventory.Count; i++)
        {
            GameObject item = camelInventory[i];

            //if its a placeholder skip
            if (item.tag == "placeholder")
                continue;

            //add items price to total
            totalItemValue += item.GetComponent<ItemValues>().GetPrice ();

            //create new placeholder
            GameObject newPlaceholder = Instantiate(placeholder, new Vector3(0,0,0), item.transform.rotation, gameObject.transform) as GameObject;

            //position place holder where item used to be on camel
            SwapPos(newPlaceholder, item);

            //replace item with placeholder
            camelInventory[i] = newPlaceholder;

            //make item invisible and uninteractable
            item.SetActive(false);

        }


        levelController.AddCoins(totalItemValue);

        //finish level
        if(numItemsInInventory == 0 || levelController.GetCoinCount() >= levelController.GetCoinGoal())
        {
            Debug.Log("Items = 0");
            levelController.LevelFinished();
            yield break;
        }

        currentCamelInventoryWeight = 0;
        levelController.SetWeight(0);   //set the weight to 0
        numberItemsInCamelInventory = 0;
        weightText.text = currentCamelInventoryWeight.ToString() + "/20";
        yield return new WaitForSeconds(1f);

        camelMoving.StartMoving();     //let camel move back
    }

    // Check if items can be put in inventory with each other
    private bool ItemsCompatable(GameObject item1, GameObject item2)
    {
        // Get items type
        ItemValues.ItemType item1Type = item1.GetComponent<ItemValues>().GetItemType();
        ItemValues.ItemType item2Type = item2.GetComponent<ItemValues>().GetItemType();

        // Compare items
        switch (item1Type)
        {
            // Normal items go with anything
            case ItemValues.ItemType.Normal:
                return true;
            // Cold items can't go with hot items but can go with other items
            case ItemValues.ItemType.Cold:
                if (item2Type == ItemValues.ItemType.Hot)
                    return false;
                return true;
            // Vice Versa
            case ItemValues.ItemType.Hot:
                if (item2Type == ItemValues.ItemType.Cold)
                    return false;
                return true;
            default:
                Debug.Log("Item of invalid type");
                return false;
        }
    }

	public void ToggleSwapping ()
	{
		canSwap = !canSwap;
	}

    public void SetSwap(bool swap)
    {
        canSwap = swap;
    }

    public int GetMaxWeight()
    {
        return maxWeight;
    }

    public void SetTutorialFinished(bool tutfinish)
    {
        tutorialFinished = tutfinish;
    }

    public void SetCamelClickable(bool click)
    {
        camelMoving.SetClickable(click);
    }

    public int GetNumItemsInCamelInventory()
    {
        return numberItemsInCamelInventory;
    }

    public AudioSource GetError()
    {
        return error;
    }
}