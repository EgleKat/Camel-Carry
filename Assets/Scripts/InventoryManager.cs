using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{

    public List<GameObject> inventory;
    public List<GameObject> camelInventory;

    public GameObject placeholder;

    public int maxWeight;

	private int currentInventorySize;
	private int currentCamelInventorySize;
	private bool canSwap;

    private int currentCamelInventoryWeight;

	private Moving camelMoving;
    LevelController levelController;

    //TODO: Add max weight text next to inventory
    //TODO: Coin audio when selling
    //TODO: Dont allow Items to be put on if greater than max weight
    //TODO: Add hot items, give cold and hot items attributes, cant be put next to each other

    // Use this for initialization
    void Start()
    {
        //TODO: set price and weight based on item
        foreach (GameObject item in inventory)
        {
            Debug.Log(item.name);

            ItemValues iv = item.GetComponent<ItemValues>();
               
            iv.SetPrice(500);
            iv.SetWeight(10);

        }

        currentCamelInventoryWeight = 0;
		camelMoving = GameObject.FindGameObjectWithTag ("Camel").GetComponent<Moving> ();

		currentCamelInventorySize = 0;
		currentInventorySize = inventory.Count;
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
    //TODO - whenever an item is added/removed from the box -call levelController.setWeight()
    
    // Removes item from inventory and puts it in camel
    private void MoveItemToCamel (GameObject objectToMove)
    {
		GameObject objectToMove2 = null;

        int objectWeight = objectToMove.GetComponent<ItemValues>().GetWeight();

        //if object would break the camels back don't allow adding
        //TODO: Highlight weight when user attemps this
        Debug.Log(currentCamelInventoryWeight + " " + objectWeight);
        if (currentCamelInventoryWeight + objectWeight >= maxWeight)
            return;

        Debug.Log("under weight");
		//find placeholder to swap with
		foreach (GameObject item in camelInventory) {
			if (item.tag == "placeholder") {
				objectToMove2 = item;
				break;
			}
		}

		//if no placeholder dont add item
		if (objectToMove2 == null)
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
        //TODO: Remove
		currentInventorySize--;
		currentCamelInventorySize++;
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
        //TODO: remove
        currentInventorySize++;
		currentCamelInventorySize--;

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