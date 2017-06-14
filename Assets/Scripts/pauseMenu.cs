using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class pauseMenu : MonoBehaviour
{

    public static bool skipFrame = false;

    // Game objects to set active and deactivate
    public GameObject PauseMenu;
    public GameObject OptionMenu;
    public GameObject InstructMenu;
    public GameObject StructureMenu;
    public GameObject ControlsMenu;
    public GameObject ResourceMenu;
    public GameObject RefractMenu;
    public GameObject ReflectMenu;
    public GameObject RedirectMenu;
    public GameObject BlockMenu;
    public GameObject Win;

    // Vars for menu buttons
    public Button Pause;
    public Button Resume;
    public Button Instructions;
    public Button Options;
    public Button End;

    //Vars for initially selected items
    public Button creatureMenuButton;
    public Button resourceInfoButton;
    public Button resourceBack;
    public Button reflectBack;
    public Button refractBack;
    public Button blockBack;
    public Button redirectBack;
    public Button controlBack;

    // Vars for win screen buttons
    public Button Restart;
    public Button MainMenu;

    // Vars for win screen text
    public Text winText;

    // Vars for other buttons
    public Button Back;
    public Toggle ghostLaserToggle;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
	public Slider UISlider;
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
        StructureMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        ResourceMenu.SetActive(false);
        RefractMenu.SetActive(false);
        ReflectMenu.SetActive(false);
        RedirectMenu.SetActive(false);
        BlockMenu.SetActive(false);
        Win.SetActive(false);

        // Set pause menu buttons to active
        Pause.interactable = true;
        Resume.interactable = true;
        Instructions.interactable = true;
        Options.interactable = true;
        End.interactable = true;

        // Other stuff
        //Screen.fullScreen = true;
        populateList();
		masterSlider.value = SoundManager.globalVolume;
		musicSlider.value = SoundManager.globalMusicVolume;
		sfxSlider.value = SoundManager.globalSoundsVolume;
		UISlider.value = SoundManager.globalUISoundsVolume;
		ghostLaserToggle.isOn = ghostLaser.ghostLaserActive;
    }

    void Update()
    {
        if ((Input.GetButtonDown("xboxStart1") || Input.GetButtonDown("xboxStart2") || Input.GetKeyDown(KeyCode.Escape))) {
            if (PauseMenu.activeInHierarchy) {
                resumeGame();
            } else {
                pauseGame();
            }
        }
        masterSlider.onValueChanged.AddListener(setMaster);
		musicSlider.onValueChanged.AddListener(setMusic);
		sfxSlider.onValueChanged.AddListener(setSFX);
		UISlider.onValueChanged.AddListener(setUISFX);
    }

    // -------------------------------------------------------- //
    // --- <<< --- +++ --- Button Functions --- +++ --- >>> --- //
    // -------------------------------------------------------- //
    public void pauseGame()
    {
        // Stops Update functions, essentially pausing the game
        Time.timeScale = 0F;
        if(SoundManager.globalVolume == 1)
		    SoundManager.globalVolume/=8;
        PauseMenu.SetActive(true);
        Pause.gameObject.SetActive(false);
        Pause.interactable = false;
        SoundManager.PlayUISound(inputController.UISounds[1].audioclip, .3f);
        //Dont change it just works I know its stupid
        //Options.Select();
		Pause.interactable = true;
		Resume.interactable = true;
		Options.interactable = true;
		Instructions.interactable = true;
		End.interactable = true;
		Resume.Select();
    }

    public void resumeGame()
    {
        // Sets the game time to realtime
        Time.timeScale = 1F;
		SoundManager.globalVolume = 1;
        skipFrame = true;
        SoundManager.PlayUISound(inputController.UISounds[1].audioclip, .3f);
        Pause.interactable = true;
        PauseMenu.SetActive(false);
		OptionMenu.SetActive(false);
		InstructMenu.SetActive(false);
		StructureMenu.SetActive(false);
		ControlsMenu.SetActive(false);
		ResourceMenu.SetActive(false);
		RefractMenu.SetActive(false);
		ReflectMenu.SetActive(false);
		RedirectMenu.SetActive(false);
		BlockMenu.SetActive(false);
    }

    public void optionsMenu()
    {
        OptionMenu.SetActive(true);
        InstructMenu.SetActive(false);
        StructureMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        ResourceMenu.SetActive(false);
        RefractMenu.SetActive(false);
        ReflectMenu.SetActive(false);
        RedirectMenu.SetActive(false);
        BlockMenu.SetActive(false);
        SoundManager.PlayUISound(inputController.UISounds[1].audioclip, .3f);
		ghostLaserToggle.Select();

        // Disable pause menu buttons when options are up
        Pause.gameObject.SetActive(false);
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
        StructureMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        ResourceMenu.SetActive(false);
        RefractMenu.SetActive(false);
        ReflectMenu.SetActive(false);
        RedirectMenu.SetActive(false);
        BlockMenu.SetActive(false);
        SoundManager.PlayUISound(inputController.UISounds[1].audioclip, .3f);
        creatureMenuButton.Select();

        // Disable pause menu buttons when instructions are up
        Pause.gameObject.SetActive(false);
        Pause.interactable = false;
        Resume.interactable = false;
        Instructions.interactable = false;
        Options.interactable = false;
        End.interactable = false;
    }

    public void structuresMenu()
    {
        InstructMenu.SetActive(false);
        OptionMenu.SetActive(false);
        StructureMenu.SetActive(true);
        ControlsMenu.SetActive(false);
        ResourceMenu.SetActive(false);
        RefractMenu.SetActive(false);
        ReflectMenu.SetActive(false);
        RedirectMenu.SetActive(false);
        BlockMenu.SetActive(false);
        SoundManager.PlayUISound(inputController.UISounds[1].audioclip, .3f);
        resourceInfoButton.Select();

        // Disable pause menu buttons when structures are up
        Pause.gameObject.SetActive(false);
        Pause.interactable = false;
        Resume.interactable = false;
        Instructions.interactable = false;
        Options.interactable = false;
        End.interactable = false;
    }

    public void controlsMenu()
    {
        InstructMenu.SetActive(false);
        OptionMenu.SetActive(false);
        StructureMenu.SetActive(false);
        ControlsMenu.SetActive(true);
        ResourceMenu.SetActive(false);
        RefractMenu.SetActive(false);
        ReflectMenu.SetActive(false);
        RedirectMenu.SetActive(false);
        BlockMenu.SetActive(false);
        SoundManager.PlayUISound(inputController.UISounds[1].audioclip, .3f);
        controlBack.Select();

        // Disable pause menu buttons when structures are up
        Pause.gameObject.SetActive(false);
        Pause.interactable = false;
        Resume.interactable = false;
        Instructions.interactable = false;
        Options.interactable = false;
        End.interactable = false;
    }

    public void resourceMenu()
    {
        InstructMenu.SetActive(false);
        OptionMenu.SetActive(false);
        StructureMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        ResourceMenu.SetActive(true);
        RefractMenu.SetActive(false);
        ReflectMenu.SetActive(false);
        RedirectMenu.SetActive(false);
        BlockMenu.SetActive(false);
        SoundManager.PlayUISound(inputController.UISounds[1].audioclip, .3f);
        resourceBack.Select();

        // Disable pause menu buttons when structures are up
        Pause.gameObject.SetActive(false);
        Pause.interactable = false;
        Resume.interactable = false;
        Instructions.interactable = false;
        Options.interactable = false;
        End.interactable = false;
    }

    public void refractMenu()
    {
        InstructMenu.SetActive(false);
        OptionMenu.SetActive(false);
        StructureMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        ResourceMenu.SetActive(false);
        RefractMenu.SetActive(true);
        ReflectMenu.SetActive(false);
        RedirectMenu.SetActive(false);
        BlockMenu.SetActive(false);
        SoundManager.PlayUISound(inputController.UISounds[1].audioclip, .3f);
        refractBack.Select();

        // Disable pause menu buttons when structures are up
        Pause.gameObject.SetActive(false);
        Pause.interactable = false;
        Resume.interactable = false;
        Instructions.interactable = false;
        Options.interactable = false;
        End.interactable = false;
    }

    public void reflectMenu()
    {
        InstructMenu.SetActive(false);
        OptionMenu.SetActive(false);
        StructureMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        ResourceMenu.SetActive(false);
        RefractMenu.SetActive(false);
        ReflectMenu.SetActive(true);
        RedirectMenu.SetActive(false);
        BlockMenu.SetActive(false);
        SoundManager.PlayUISound(inputController.UISounds[1].audioclip, .3f);
        reflectBack.Select();

        // Disable pause menu buttons when structures are up
        Pause.gameObject.SetActive(false);
        Pause.interactable = false;
        Resume.interactable = false;
        Instructions.interactable = false;
        Options.interactable = false;
        End.interactable = false;
    }

    public void redirectMenu()
    {
        InstructMenu.SetActive(false);
        OptionMenu.SetActive(false);
        StructureMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        ResourceMenu.SetActive(false);
        RefractMenu.SetActive(false);
        ReflectMenu.SetActive(false);
        RedirectMenu.SetActive(true);
        BlockMenu.SetActive(false);
        SoundManager.PlayUISound(inputController.UISounds[1].audioclip, .3f);
        redirectBack.Select();

        // Disable pause menu buttons when structures are up
        Pause.gameObject.SetActive(false);
        Pause.interactable = false;
        Resume.interactable = false;
        Instructions.interactable = false;
        Options.interactable = false;
        End.interactable = false;
    }

    public void blockMenu()
    {
        InstructMenu.SetActive(false);
        OptionMenu.SetActive(false);
        StructureMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        ResourceMenu.SetActive(false);
        RefractMenu.SetActive(false);
        ReflectMenu.SetActive(false);
        RedirectMenu.SetActive(false);
        BlockMenu.SetActive(true);
        SoundManager.PlayUISound(inputController.UISounds[1].audioclip, .3f);
        blockBack.Select();

        // Disable pause menu buttons when structures are up
        Pause.gameObject.SetActive(false);
        Pause.interactable = false;
        Resume.interactable = false;
        Instructions.interactable = false;
        Options.interactable = false;
        End.interactable = false;
    }

    public void endGame()
    {
        // Resets the time scale so the game doesn't freeze when restarted
		SoundManager.StopAll();
        SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        Time.timeScale = 1F;
        PauseMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        Win.SetActive(false);
    }

    public void goBack()
    {
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        StructureMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        ResourceMenu.SetActive(false);
        RefractMenu.SetActive(false);
        ReflectMenu.SetActive(false);
        RedirectMenu.SetActive(false);
        BlockMenu.SetActive(false);

        // Set pause menu buttons to active
        //Pause.interactable = true;
        Resume.interactable = true;
        Instructions.interactable = true;
        Options.interactable = true;
        End.interactable = true;
        SoundManager.PlayUISound(inputController.UISounds[1].audioclip, .3f);
        Resume.Select();

    }

    // Call winGame in gridManager in applyDamage and input "P1" or "P2"
    public void winGame(Player player)
    {
        // Stops Update functions, essentially pausing the game
        Time.timeScale = 0F;
        // Take the in winner value and display correct winner text
		if (player == Player.PlayerOne) { winText.text = "Player 1 Wins!"; winText.color = Color.red; }
		else if (player == Player.PlayerTwo) { winText.text = "Player 2 Wins!"; winText.color = Color.green; }
        Restart.enabled = false;
        MainMenu.enabled = false;
        Win.SetActive(true);
        StartCoroutine(winGameMenu());
    }

    private IEnumerator winGameMenu()
    {
        yield return new WaitForSecondsRealtime(2f);
        Restart.enabled = true;
        MainMenu.enabled = true;
        Restart.Select();
    }

    // Function for the "Play Again" button
    public void restartGame()
    {
        // Resets the time scale so the game doesn't freeze when restarted
        SceneManager.LoadScene("main", LoadSceneMode.Single);
        /*Time.timeScale = 1F;
        PauseMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        Win.SetActive(false);*/
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
	public void setUISFX(float vol)
	{
		SoundManager.globalUISoundsVolume = vol;
		//Need actual sound source to implement
	}

    public void toggleGhostLaser()
    {
		ghostLaser.ghostLaserActive = ghostLaserToggle.isOn;
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
        //resDrop.AddOptions(resos);
    }
}
