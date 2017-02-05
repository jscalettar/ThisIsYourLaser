using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class startMenu : MonoBehaviour {

    public Button Play;
    public Button Exit;

	// Use this for initialization
	void Start () {

        Play = Play.GetComponent<Button>();
        Exit = Exit.GetComponent<Button>();
	}
	
	public void startGame()
    {
        SceneManager.LoadScene("main");
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
