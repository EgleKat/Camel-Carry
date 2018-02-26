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

    public int maxWeight;
	private bool canSwap;

    private int currentCamelInventoryWeight;
    private TextMeshProUGUI weightText;

    private Moving camelMoving;
    LevelController levelController;

    //TODO: Coin audio when selling

    // Use this for initialization
    void Start()
    {

        sellItems = GetComponent<AudioSource>();
        foreach (GameObject item in inventory)
        {
            Debug.Log(item.name);
            ItemValues itemVals = item.GetComponent<ItemValues>();

            //customisable item and weight for each item
            //items are organised into types: normal, cold and hot
            switch (item.name)
            {
                case "Umbrella":
                    itemVals.SetPrice(500);
                    itemVals.SetWeight(10);
                    goto case "Normal";
                case "Sword":
                    itemVals.SetPrice(500);
                    itemVals.SetWeight(10);
                    goto case "Normal";
                case "Flip-Flops":
                    itemVals.SetPrice(500);
                    itemVals.SetWeight(10);
                    goto case "Normal";
                case "Mysterious-bottle":
                    itemVals.SetPrice(500);
                    itemVals.SetWeight(10);
                    goto case "Normal";
                case "Normal":
                    itemVals.SetType(ItemValues.ItemType.Normal);
                    break;

                case "Ice Cream":
                    itemVals.SetPrice(500);
                    itemVals.SetWeight(10);
                    goto case "Cold";
                case "Ice Cube":
                    itemVals.SetPrice(500);
                    itemVals.SetWeight(10);
                    goto case "Cold";
                case "Cold":
                    itemVals.SetType(ItemValues.ItemType.Cold);
                    break;

                case "Microwave":
                    itemVals.SetPrice(500);
                    itemVals.SetWeight(10);
                    goto case "Hot";
                case "Hot":
                    itemVals.SetType(ItemValues.ItemType.Hot);
                    break;

                default:
                    Debug.Log("Item Not Recognised");
                    break;
            }

        }

        weightText = GameObject.Find("MaxWeight").GetComponent<TextMeshProUGUI>();
        weightText.text = "0";

        currentCamelInventoryWeight = 0;
		camelMoving = GameObject.FindGameObjectWithTag ("Camel").GetComponent<Moving> ();

        levelController = GameObject.FindWithTag("GameController").GetComponent<LevelController>();
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

        int objectWeight = objectToMove.GetComponent<ItemValues>().GetWeight();

        Debug.Log("under weight");
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
        Debug.Log(currentCamelInventoryWeight + " " + objectWeight);
        if (currentCamelInventoryWeight + objectWeight > maxWeight)
        {
            //flash max weight red
            StartCoroutine(GameObject.Find("MaxWeightBorder").GetComponent<FlashBorder>().FlashBorderRed());
            shouldReturn = true;
        }

        if (shouldReturn)
            return;

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

        Debug.Log(levelController.GetWeight());

        weightText.text = currentCamelInventoryWeight.ToString();
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

        Debug.Log(levelController.GetWeight());

        weightText.text = currentCamelInventoryWeight.ToString();

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

	public void SellItems ()
	{
       
		int totalItemValue = 0;
        //loop through all items in camels inventory
        for (int i = 0; i < camelInventory.Count - 1; i++)
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

        sellItems.Play();

        Debug.Log(totalItemValue);
        levelController.AddCoins(totalItemValue);

        //finish level
        if(inventory.Count == 0)
        {
            levelController.LevelFinished();
        }

        currentCamelInventoryWeight = 0;
        levelController.SetWeight(0);   //set the weight to 0
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
}