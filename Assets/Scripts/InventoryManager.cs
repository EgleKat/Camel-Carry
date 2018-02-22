using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public List<GameObject> inventory;
    public List<GameObject> camelInventory;

	private int currentInventorySize;
	private int currentCamelInventorySize;
	private bool canSwap;

	private Moving camelMoving;
    LevelController levelController;



    // Use this for initialization
    void Start()
    {
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
	private void swapPos (GameObject a, GameObject b)
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

		swapPos (objectToMove, objectToMove2);

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

		swapPos (objectToMove, objectToMove2);

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
        levelController.SetWeight(0);   //set the weight to 0
        levelController.AddCoins(50);
        camelMoving.StartMoving ();     //let camel move back
	}

	public void ToggleSwapping ()
	{
		canSwap = !canSwap;
	}
}