using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserControlls : MonoBehaviour {

    private bool instructedToRestart;
	// Use this for initialization
	void Awake () {
        instructedToRestart = false;
        DontDestroyOnLoad(this);
    }

    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
