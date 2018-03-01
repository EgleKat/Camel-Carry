using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemValues : MonoBehaviour {

    private TextMeshProUGUI price;
    private TextMeshProUGUI weight;

    private int priceVal;
    private int weightVal;
    public enum ItemType {Normal, Hot, Cold};

    private ItemType thisItemType;

    public int itemMultiplier;

    // Use this for initialization
    void Awake () {
        price = transform.Find ("Price").gameObject.GetComponent<TextMeshProUGUI> ();
        weight = transform.Find ("Weight").gameObject.GetComponent <TextMeshProUGUI> ();

        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPrice (int priceToSet)
    {
        priceVal = priceToSet * itemMultiplier;
        price.text = priceVal.ToString ();

    }

    public int GetPrice()
    {
        return priceVal;
    }

    public void SetWeight(int weightToSet)
    {
        weightVal = weightToSet * itemMultiplier;
        weight.text = weightVal.ToString();

    }

    public int GetWeight()
    {
        return weightVal;
    }

    public void SetType(ItemType type)
    {
        thisItemType = type;
    }

    public ItemType GetItemType()
    {
        return thisItemType;
    }
}
