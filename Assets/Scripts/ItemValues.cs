using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemValues : MonoBehaviour {

    private TextMeshProUGUI price;
    private TextMeshProUGUI weight;

    private int priceVal;
    private int weightVal;

    // Use this for initialization
    void Start () {
        price = gameObject.transform.Find ("Price").gameObject.GetComponent<TextMeshProUGUI> ();
        weight = gameObject.transform.Find ("Weight").gameObject.GetComponent <TextMeshProUGUI> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPrice (int priceToSet)
    {
        priceVal = priceToSet;
        price.text = priceToSet.ToString ();

    }

    public int GetPrice()
    {
        return priceVal;
    }

    public void SetWeight(int weightToSet)
    {
        weightVal = weightToSet;
        weight.text = weightToSet.ToString();

    }

    public int GetWeight()
    {
        return weightVal;
    }
}
