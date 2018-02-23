﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{

    public List<GameObject> inventory;
    public List<GameObject> camelInventory;

    public GameObject placeholder;

	private int currentInventorySize;
	private int currentCamelInventorySize;
	private bool canSwap;

	private Moving camelMoving;
    LevelController levelController;

    // Use this for initialization
    void Start()
    {
        //TODO: set price and weight based on item
        foreach (GameObject item in inventory)
        {
            Debug.Log(item.name);

            ItemValues iv = item.GetComponent<ItemValues>();
               
            iv.SetPrice(500);
            iv.SetWeight(20);

        }

		camelMoving = GameObject.FindGameObjectWithTag ("Camel").GetComponent<Moving> ();

		currentCamelInventorySize = 0;
		currentInventorySize = inventory.Count;
        levelController = GameObject.FindWithTag("GameController").GetComponent<LevelController>();
        levelController.SetWeight(0);   //set the weight to 0
		canSwap = false;
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

		//move items
		int object2Index = camelInventory.IndexOf (objectToMove2);
		int object1Index = inventory.IndexOf (objectToMove);

		inventory[object1Index] = objectToMove2;
		camelInventory[object2Index] = objectToMove;

		SwapPos (objectToMove, objectToMove2);

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

		currentInventorySize++;
		currentCamelInventorySize--;

    }


    public void ToggleInventory (GameObject objectToMove)
    {
		if (canSwap)
			return;
		
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
        //TODO change the 0 to the weight value, and coin value
		//TODO call level controller to finish level if all items are sold

		int totalItemValue = 0;
        for (int i = 0; i < camelInventory.Count - 1; i++)
        {
            GameObject item = camelInventory[i];

            if (item.tag == "placeholder")
                continue;

            totalItemValue += item.GetComponent<ItemValues>().GetPrice ();

            Debug.Log(item.name);
            Debug.Log(item.transform.localPosition);
            GameObject newPlaceholder = Instantiate(placeholder, new Vector3(0,0,0), item.transform.rotation, gameObject.transform) as GameObject;

            SwapPos(newPlaceholder, item);
            camelInventory[i] = newPlaceholder;
            item.SetActive(false);
        }

        Debug.Log(totalItemValue);
        levelController.AddCoins(totalItemValue);

        levelController.SetWeight(0);   //set the weight to 0
        camelMoving.StartMoving();     //let camel move back
    }

	public void ToggleSwapping ()
	{
		canSwap = !canSwap;
	}
}