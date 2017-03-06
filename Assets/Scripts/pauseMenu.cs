using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    // Game objects to set active and deactivate
    public GameObject PauseMenu;
    public GameObject OptionMenu;
    public GameObject InstructMenu;

    // Vars for menu buttons
    public Button Pause;
    public Button Resume;
    public Button Instructions;
    public Button Options;
    public Button End;

    // Vars for other buttons
    public Button Back;
    public Toggle fullscreenToggle;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    //Vars for screen options
    public bool isFull;
    public Dropdown resDrop;
    List<string> resos = new List<string>() { "800x600", "1024x768", "1280x720", "1920x1080" };

    //Vars for sounds options
    public float masterVol;
    public float musicVol;
    public float sfxVol;

    // Use this for initialization
    void Start()
    {
        // Set the menus to inactive
        PauseMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);

        // Set pause menu buttons to active
        Pause.interactable = true;
        Resume.interactable = true;
        Instructions.interactable = true;
        Options.interactable = true;
        End.interactable = true;

        // Other stuff
        Screen.fullScreen = false;
        populateList();
    }

    void Update()
    {
        //setMaster(masterSlider.value);
        //setMusic(musicSlider.value);
        //setSFX(sfxSlider.value);
    }

    // -------------------------------------------------------- //
    // --- <<< --- +++ --- Button Functions --- +++ --- >>> --- //
    // -------------------------------------------------------- //
    public void pauseGame()
    {
        // Stops Update functions, essentially pausing the game
        Time.timeScale = 0F;
        PauseMenu.SetActive(true);
    }

    public void resumeGame()
    {
        // Sets the game time to realtime
        Time.timeScale = 1F;
        PauseMenu.SetActive(false);
    }

    public void optionsMenu()
    {
        OptionMenu.SetActive(true);
        InstructMenu.SetActive(false);

        // Disable pause menu buttons when options are up
        Pause.interactable = false;
        Resume.interactable = false;
        Instructions.interactable = false;
        Options.interactable = false;
        End.interactable = false;
    }

    public void instructionsMenu()
    {
        InstructMenu.SetActive(true);
        OptionMenu.SetActive(false);

        // Disable pause menu buttons when instructions are up
        Pause.interactable = false;
        Resume.interactable = false;
        Instructions.interactable = false;
        Options.interactable = false;
        End.interactable = false;
    }

    public void endGame()
    {
        // Resets the time scale so the game doesn't freeze when restarted
        SceneManager.LoadScene("TitleScreen");
        Time.timeScale = 1F;
        PauseMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
    }

    public void goBack()
    {
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);

        // Set pause menu buttons to active
        Pause.interactable = true;
        Resume.interactable = true;
        Instructions.interactable = true;
        Options.interactable = true;
        End.interactable = true;
    }

    // ------------------------------------------------------- //
    // --- <<< --- +++ --- Other Functions --- +++ --- >>> --- //
    // ------------------------------------------------------- //
    public void setMaster(float vol)
    {
        SoundManager.globalVolume = vol;
        //Need actual sound source to implement
    }

    public void setMusic(float vol)
    {
        SoundManager.globalMusicVolume = vol;
        //Need actual sound source to implement
    }

    public void setSFX(float vol)
    {
        SoundManager.globalSoundsVolume = vol;
        //Need actual sound source to implement
    }

    public void toggleFullscreen()
    {
        isFull = Screen.fullScreen = !Screen.fullScreen;
    }

    public void changeRes(int num)
    {
        switch (num)
        {
            case 0:
                Screen.SetResolution(800, 600, true);
                break;
            case 1:
                Screen.SetResolution(1024, 768, true);
                break;
            case 2:
                Screen.SetResolution(1280, 720, true);
                break;
            case 3:
                Screen.SetResolution(1920, 1080, true);
                break;
        }
    }

    public void populateList()
    {
        resDrop.AddOptions(resos);
    }
}
