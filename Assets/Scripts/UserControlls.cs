using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserControlls : MonoBehaviour
{

    private bool instructedToRestart; float lastTime;

    // Use this for initialization
    void Awake()
    {
        instructedToRestart = false;
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        lastTime = Time.time;

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && (Time.time - lastTime > 4.0f))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            lastTime = Time.time;

        }
    }



    public bool GetInstructedToRestart()
    {
        return instructedToRestart;
    }

    public void SetInstructedToRestart(bool restart)
    {
        instructedToRestart = restart;
    }
}
