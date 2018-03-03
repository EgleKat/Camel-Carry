using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBorder : MonoBehaviour {

    private bool isHighlighted;
	// Use this for initialization
	void Start () {
        //hide
        GetComponent<CanvasRenderer>().SetAlpha(0);
        isHighlighted = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator FlashBorderRed()
    {
        if (isHighlighted)
        {
            yield break;
        }
        int i = 0;
        while (i < 2)
        {
            //show, pause, hide, pause ...etc
            GetComponent<CanvasRenderer>().SetAlpha(1f);
            yield return new WaitForSeconds(0.3f);
            GetComponent<CanvasRenderer>().SetAlpha(0);
            yield return new WaitForSeconds(0.3f);
            i++;
        }
    }

    public void Highlight()
    {
        GetComponent<CanvasRenderer>().SetAlpha(1);
        isHighlighted = true;
    }

    public void Hide()
    {
        GetComponent<CanvasRenderer>().SetAlpha(0);
        isHighlighted = false;
    }
}
