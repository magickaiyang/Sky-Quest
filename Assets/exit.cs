using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Exit()
    {
        Application.Quit();
    }

    public void Begin()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("layout", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
