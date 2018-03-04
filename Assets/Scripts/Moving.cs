using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Moving : MonoBehaviour
{

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
    AudioSource walkingAudio;
    AudioSource standingAudio;
    private Transform chestTransform;
    private InventoryManager inventoryManager;
    private Tutorial tutorial;
    //int secondsToFade = 6;



    // Use this for initialization
    void Start()
    {
        tutorial = GameObject.Find("Tutorial").GetComponent<Tutorial>();
        camelAudio = GetComponents<AudioSource>()[1];
        walkingAudio = GetComponents<AudioSource>()[0];
        standingAudio = GetComponents<AudioSource>()[2];

        rb = GetComponent<Rigidbody>();
        chestTransform = GameObject.FindWithTag("chest").transform;
        animator = GetComponent<Animator>();
        chestAnimator = GameObject.FindWithTag("chest_top").GetComponent<AnimateChest>();
        inventoryManager = GameObject.FindGameObjectWithTag("inventory_manager").GetComponent<InventoryManager>();
        levelController = GameObject.FindWithTag("GameController").GetComponent<LevelController>();

        clickable = false;
        stopped = true;
        firstTime = false;
        animator.SetBool("isWalking", false);
        standingAudio.Play();

    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        CamelBegin();
    }

    void CamelBegin()
    {
        if (clickable)
        {
            Debug.Log("clickable");
            standingAudio.Stop();
            walkingAudio.Play();
            if (!firstTime)
            {
                Debug.Log("First time");
                if (levelController.GetLevel() < 3)
                {
                    tutorial.ChangeSecondaryText();
                }
                firstTime = true;
                levelController.StartTimer();
                // StartMoving();
            }

            //start playing audio from 1 second
            camelAudio.time = 1;
            camelAudio.Play();

            clickable = false;
            inventoryManager.ToggleSwapping();

            //start the animation
            animator.SetBool("isWalking", true);
            StartMoving();
            direction = Vector3.right;
        }
        if (!clickable && inventoryManager.GetNumItemsInCamelInventory() == 0)
        {
            StartCoroutine(tutorial.GetBorder("InventoryBorder").FlashBorderRed());
            inventoryManager.GetError().Play();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!clickable && !stopped)
        {
            rb.velocity = direction * Time.deltaTime * speedMultiplier;
        }
        //   if (walkingAudio.volume < 0.7)
        //   {
        //     walkingAudio.volume = walkingAudio.volume + (Time.deltaTime / (secondsToFade + 1));
        //     standingAudio.volume = standingAudio.volume + (Time.deltaTime / (secondsToFade + 1));
        //
        //   }


    }
    private void Update()
    {
        //on space, start moving he camel
        if (Input.GetKeyDown("space"))
        {
            CamelBegin();
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        //When camel reaches the collider, it stops
        StopMoving(true);

        //Rotate the camel and the chest
        rb.rotation = rb.rotation * Quaternion.Euler(0, 180, 0);
        chestTransform.localRotation = chestTransform.localRotation * Quaternion.Euler(0, 180, 0);

        //if camel hits the market
        if (other.gameObject.name == "Market")
        {
            //Change the moving direction
            direction = Vector3.left;
            //Send a message to inventory_manager
            StartCoroutine(inventoryManager.SellItems());
        }
        //if camel hits beginning marker
        else
        {
            //let the user click the camel
            inventoryManager.ToggleSwapping();
            walkingAudio.Stop();
            standingAudio.Play();

        }

    }

    public void StopMoving(bool animateChest)
    {
        Debug.Log("Stopped Moving");
        stopped = true;
        if (animateChest)
        {
            chestAnimator.SetAnimation(stopped);
        }
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
        speedMultiplier -= weight * 5;

        //Speed can only be 0 or greater than 0
        if (speedMultiplier < 0)
            speedMultiplier = 0;

        animator.speed = speedMultiplier * 0.01f;
    }

    public void SetClickable(bool value)
    {
        clickable = value;
    }
    public void TurnAllMusicOff()
    {
        walkingAudio.Stop();
        standingAudio.Stop();

    }
}
