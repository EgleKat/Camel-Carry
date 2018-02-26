using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBorder : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //hide
        GetComponent<CanvasRenderer>().SetAlpha(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator FlashBorderRed()
    {
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
}
