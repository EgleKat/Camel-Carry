﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemValues : MonoBehaviour {

    private TextMeshProUGUI price;
    private TextMeshProUGUI weight;

    private int priceVal;
    private int weightVal;

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
        priceVal = priceToSet;
        price.text = priceToSet.ToString ();
        Debug.Log("Set Price");

    }

    public int GetPrice()
    {
        return priceVal;
    }

    public void SetWeight(int weightToSet)
    {
        weightVal = weightToSet;
        weight.text = weightToSet.ToString();
        Debug.Log("Set Weight");

    }

    public int GetWeight()
    {
        return weightVal;
    }
}
