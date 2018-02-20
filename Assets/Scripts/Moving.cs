using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour {

    private Rigidbody rb;
    public float speedMultiplier;
    private bool clickable; //can the user click on the camel
    Animator animator;
    Vector3 direction;  //left or right
    public bool firstTime;  //first time the camel moves, i.e. start timer then
    private bool stopped;
    LevelController levelController;
    AudioSource audio;


    // Use this for initialization
    void Start () {
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        clickable = true;
        stopped = true;
        animator.SetBool("isWalking", false);
        firstTime = false;
        levelController = GameObject.FindWithTag("GameController").GetComponent<LevelController>();

    }

    void OnMouseDown()
    {
        Debug.Log("Mouse Down");
        if (clickable)
        {
            if (!firstTime)
            {
                firstTime = true;
                levelController.StartTimer();
                startMoving();
            }

            audio.Play();
            animator.SetBool("isWalking", true);
            direction  = Vector3.right;
            clickable = false;
            startMoving();
            Debug.Log("Clicked");
        }
    }

 

    // Update is called once per frame
    void FixedUpdate () {

        if (!clickable && !stopped)
        {
            rb.velocity = direction * Time.deltaTime * speedMultiplier;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        //When camel reaches the collider, it stops
        stopMoving();

        //Rotate the camel
        rb.rotation = rb.rotation * Quaternion.Euler(0, 180, 0);

        //if camel hits the market
        if(other.gameObject.name =="Market")
        {
            //Change the moving direction
            direction = Vector3.left;
            //TODO send a message to inventory_manager

        }
        //if camel hits beginning marker
        else
        {
            //let the user click the camel
            clickable = true;
       
        }

    }

    public void stopMoving()
    {
        Debug.Log("Stopped");
        stopped = true;
        rb.velocity = Vector3.zero;
        animator.SetBool("isWalking", false);

    }
    public void startMoving()
    {
        stopped = false;
        rb.velocity = Vector3.zero;
        animator.SetBool("isWalking", true);
    }
}
