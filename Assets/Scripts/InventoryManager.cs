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


    // Use this for initialization
    void Start()
    {
		currentCamelInventorySize = 0;
		currentInventorySize = inventory.Count;
    }

    // Update is called once per frame
    void Update()
    {

    }

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
		camelMoving.StartMoving ();
	}
}