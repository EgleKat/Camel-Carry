using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserControlls : MonoBehaviour {

    private bool instrutedToRestart;
	// Use this for initialization
	void Awake () {
        instrutedToRestart = false;
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
        return instrutedToRestart;
    }

    public void SetInstructedToRestart(bool restart)
    {
        instrutedToRestart = restart;
    }
}
