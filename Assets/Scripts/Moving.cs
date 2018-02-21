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
    AnimateChest chestAnimator;
    AudioSource camelAudio;
    private Transform chestTransform;
    private InventoryManager inventoryManager;


    // Use this for initialization
    void Start () {
        camelAudio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        chestTransform = GameObject.FindWithTag("chest").transform;
        animator = GetComponent<Animator>();
        chestAnimator = GameObject.FindWithTag("chest_top").GetComponent<AnimateChest>();
        inventoryManager = GameObject.FindGameObjectWithTag("inventory_manager").GetComponent<InventoryManager>();
        levelController = GameObject.FindWithTag("GameController").GetComponent<LevelController>();

        clickable = true;
        stopped = true;
        firstTime = false;
        animator.SetBool("isWalking", false);


    }

    void OnMouseDown()
    {
        if (clickable)
        {
            if (!firstTime)
            {
                firstTime = true;
                levelController.StartTimer();
                StartMoving();
            }

            //start playing audio from 1 second
            camelAudio.time = 1;
            camelAudio.Play();

            clickable = false;
			inventoryManager.ToggleSwapping ();

            //start the animation
            animator.SetBool("isWalking", true);
            direction  = Vector3.right;
            StartMoving();
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
        StopMoving();

        //Rotate the camel and the chest
        rb.rotation = rb.rotation * Quaternion.Euler(0, 180, 0);
        chestTransform.localRotation = chestTransform.localRotation * Quaternion.Euler(0, 180,0);

        //if camel hits the market
        if (other.gameObject.name =="Market")
        {
            //Change the moving direction
            direction = Vector3.left;
            //Send a message to inventory_manager
            inventoryManager.SellItems();
        }
        //if camel hits beginning marker
        else
        {
            //let the user click the camel
            clickable = true;
			inventoryManager.ToggleSwapping ();
       		
        }

    }

    public void StopMoving()
    {
        Debug.Log("Stopped");
        stopped = true;
        chestAnimator.SetAnimation(stopped);
        rb.velocity = Vector3.zero;
        animator.SetBool("isWalking", false);

    }
    public void StartMoving()
    {
        changeSpeed();
        stopped = false;
        chestAnimator.SetAnimation(stopped);
        rb.velocity = Vector3.zero; //need this here so that speed isn't skewed by Time.delta
        animator.SetBool("isWalking", true);

    }
    private void changeSpeed()
    {
        int weight = levelController.GetWeight();
        if (weight == 0)
        {
            speedMultiplier = 100;
        }
        speedMultiplier -= weight*2;
        animator.speed = speedMultiplier * 0.01f;
    }
}
