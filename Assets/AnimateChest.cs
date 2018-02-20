using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateChest : MonoBehaviour {

    Animator animator; 
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        animator.SetBool("camelStopped", true);
	}

    public void SetAnimation(bool stopped)
    {
        animator.SetBool("camelStopped", stopped);

    }



}
