using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashboard_Manager : MonoBehaviour
{

    public List<GameObject> inventory;
    public List<GameObject> camel;

    public GameObject inventory_object;
    public GameObject camel_object;

    private RectTransform inventory_transform;
    private RectTransform camel_transform;

    // Distance between inventory slots
    private float col_multiplier_inventory;
    private float row_multiplier;

    // Distance between camel slots
    private float col_multiplier_camel;

    private float first_item_inventory_x;
    private float first_item_inventory_y;

    private float first_item_camel_x;
    private float first_item_camel_y;

    public int camel_list_size;
    public int inventory_list_size;


    // Use this for initialization
    void Start()
    {
        inventory_transform = inventory_object.GetComponent<RectTransform>();
        camel_transform = camel_object.GetComponent<RectTransform>();

        // CHANGE IF NUMBER OF BOXES CHANGE!!
        // Relative positions
        col_multiplier_inventory = inventory_transform.sizeDelta.x / 3;
        col_multiplier_camel = camel_transform.sizeDelta.x / 2;
        row_multiplier = inventory_transform.sizeDelta.y / 2;

        // Positions of first items
        first_item_inventory_x = inventory_transform.position.x - col_multiplier_inventory;
        first_item_inventory_y = inventory_transform.position.y + row_multiplier / 2;

        first_item_camel_x = camel_transform.position.x - col_multiplier_camel / 2;
        first_item_camel_y = camel_transform.position.y + row_multiplier / 2;

        SetUpInventory();

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Removes item from inventory and puts it in camel
    private void MoveItemToCamel(GameObject object_to_move)
    {
        if (camel.Count >= camel_list_size)
            return;

        inventory.Remove(object_to_move);
        camel.Add(object_to_move);

        SetUpInventory();
    }

    // Removes item from camel and puts it in inventory
    private void MoveItemToInventory(GameObject object_to_move)
    {
        if (inventory.Count >= inventory_list_size)
            return;

        camel.Remove(object_to_move);
        inventory.Add(object_to_move);

        SetUpInventory();
    }

    private void SetUpInventory()
    {
        int current_row = 0;
        int current_col = 0;

        foreach (GameObject item in inventory)
        {
            if (item == null)
                continue;

            // Item positions
            float item_x = first_item_inventory_x + current_col * col_multiplier_inventory;
            float item_y = first_item_inventory_y - current_row * row_multiplier;

            item.transform.position = new Vector3(item_x, item_y, 0);
            item.SetActive(true);

            current_col++;

            if (current_col > inventory_list_size / 2 - 1)
            {
                current_col = 0;
                current_row++;
            }
        }

        current_row = 0;
        current_col = 0;

        foreach (GameObject item2 in camel)
        {
            if (item2 == null)
                continue;

            // Item positions
            float item_x = first_item_camel_x + current_col * col_multiplier_camel;
            float item_y = first_item_camel_y - current_row * row_multiplier;


            item2.transform.position = new Vector3(item_x, item_y, 0);
            item2.SetActive(true);

            current_col++;

            if (current_col > camel_list_size / 2 - 1)
            {
                current_col = 0;
                current_row++;
            }
        }
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
}