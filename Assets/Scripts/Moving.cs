using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour {

    private Rigidbody rb;
    public float speedMultiplier;
    private bool clickable;
    Animator animator;
    Vector3 direction;
    public bool firstTime;
    LevelController levelController;


    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        clickable = true;
        animator.SetBool("isWalking", false);
        firstTime = false;
        levelController = GameObject.FindWithTag("GameController").GetComponent<LevelController>();

    }

    void OnMouseDown()
    {
        if (!firstTime)
        {
            firstTime = true;
            levelController.StartTimer();
        }

        Debug.Log("Mouse Down");
        if (clickable)
        {
            animator.SetBool("isWalking", true);
            direction  = Vector3.right;
            clickable = false;
            Debug.Log("Clicked");
        }
    }

 

    // Update is called once per frame
    void FixedUpdate () {

        if (!clickable)
        {
            rb.velocity = direction * Time.deltaTime * speedMultiplier;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        //When camel reaches the collider, it stops
        rb.velocity = Vector3.zero;
        animator.SetBool("isWalking", false);

        //Rotate the camel
        rb.rotation = rb.rotation * Quaternion.Euler(0, 180, 0);

        //if camel hits the market
        if(other.gameObject.name =="Market")
        {
            Debug.Log("It's market");
            //Move camel back
            direction = Vector3.left;
            animator.SetBool("isWalking", true);

        }
        //if camel hits beginning marker
        else
        {
            Debug.Log("Back at the start");
            //let the user click the camel
            clickable = true;
       
        }

    }

}
