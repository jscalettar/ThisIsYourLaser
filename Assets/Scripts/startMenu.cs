using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class startMenu : MonoBehaviour {
    //Hold game objects to show/hide as neccesary
    public GameObject StartMenu;
    public GameObject OptionMenu;

    //Vars for buttons
    public Button Play;
    public Button Exit;

	// Use this for initialization
	void Start () {
        StartMenu.SetActive(true);
        OptionMenu.SetActive(false);
    }
	
	public void startGame()
    {
        SceneManager.LoadScene("main");
    }
    public void mainMenu() {
        StartMenu.SetActive(true);
        OptionMenu.SetActive(false);
    }

    public void OptionsMenu()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(true);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
