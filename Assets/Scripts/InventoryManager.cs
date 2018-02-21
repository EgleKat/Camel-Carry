using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public List<GameObject> inventory;
    public List<GameObject> camelInventory;

	private int currentInventorySize;
	private int currentCamelInventorySize;

	public Moving camelMoving;
    LevelController levelController;



    // Use this for initialization
    void Start()
    {
		currentCamelInventorySize = 0;
		currentInventorySize = inventory.Count;
        levelController = GameObject.FindWithTag("GameController").GetComponent<LevelController>();
        levelController.SetWeight(0);   //set the weight to 0
    }

    // Update is called once per frame
    void Update()
    {

    }
    //TODO - whenever an item is added/removed from the box -call levelController.setWeight()
    // Removes item from inventory and puts it in camel
    private void MoveItemToCamel(GameObject object_to_move)
    {
        if (currentCamelInventorySize >= camelInventory.Count)
            return;

        inventory.Remove(object_to_move);
        camelInventory.Add(object_to_move);

    }

    // Removes item from camel and puts it in inventory
    private void MoveItemToInventory(GameObject objectToMove)
    {
        if (currentInventorySize >= inventory.Count)
            return;

        camelInventory.Remove(objectToMove);
        inventory.Add(objectToMove);

		currentInventorySize++;

    }


    public void ToggleInventory(GameObject object_to_move)
    {
        if (inventory.Contains(object_to_move))
        {
            MoveItemToCamel(object_to_move);
        }
        else
        {
            MoveItemToInventory(object_to_move);
        }
    }

	public void SellItems()
	{
        //TODO change the 0 to the weight value, and coin value
        levelController.SetWeight(0);   //set the weight to 0
        levelController.AddCoins(50);
        camelMoving.StartMoving ();     //let camel move back
	}

}