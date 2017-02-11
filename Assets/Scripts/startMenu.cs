using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class startMenu : MonoBehaviour {
    //Vars for screen options
    public int resolution;
    public bool isFull;

    //Vars for sounds options
    public float masterVol;
    public float musicVol;
    public float sfxVol;

    //Hold game objects to show/hide as neccesary
    public GameObject StartMenu;
    public GameObject OptionMenu;
    public GameObject InstructMenu;

    //Vars for buttons/toggles/sliders
    public Button Play;
    public Button Exit;

    public Dropdown resolutionMenu;
    public Toggle fullscreenToggle;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public Resolution[] resolutions;
 
    // Use this for initialization
    void Start () {
        Screen.fullScreen = false;
        StartMenu.SetActive(true);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
    }
	
	public void startGame()
    {
        SceneManager.LoadScene("main");
    }

    public void mainMenu() {
        StartMenu.SetActive(true);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
    }

    public void OptionsMenu()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(true);
        InstructMenu.SetActive(false);
    }

    public void InstructionsMenu()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(true);
    }

    public void setMaster(float vol) 
    {
        //Need actual sound source to implement
    }

    public void setMusic(float vol) 
    {
        //Need actual sound source to implement
    }

    public void setSFX(float vol) 
    {
        //Need actual sound source to implement
    }

    public void setResolution(int res) 
    {
        switch (resolutionMenu.value) 
        {
            case 0:
                Screen.SetResolution(1366, 768, isFull);
                break;
            case 1:
                Screen.SetResolution(1600, 900, isFull);
                break;
            case 2:
                Screen.SetResolution(1920, 1080, isFull);
                break;
            default:
                Screen.SetResolution(800, 600, isFull);
                break;
        }

        
    }

    public void toggleFullscreen() 
    {
        isFull = Screen.fullScreen = !Screen.fullScreen;


    }
    public void exitGame()
    {
        Application.Quit();
    }
}
