using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateChest : MonoBehaviour {

    Animator animator;
    AudioSource audioChest;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        audioChest = GetComponent<AudioSource>();
        animator.SetBool("camelStopped", true);
	}

    public void SetAnimation(bool stopped)
    {
        animator.SetBool("camelStopped", stopped);

    }
    public void playAudio()
    {
        audioChest.Play();
    }


}
