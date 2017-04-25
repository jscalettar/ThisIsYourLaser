using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class startMenu : MonoBehaviour {
    //Vars for screen options
    public bool isFull;
    public Dropdown resDrop;
    List<string> resos = new List<string>() { "800x600", "1024x768", "1280x720", "1920x1080" };

    //Vars for sounds options
    public float masterVol;
    public float musicVol;
    public float sfxVol;

    //Hold game objects to show/hide as neccesary
    public GameObject StartMenu;
    public GameObject OptionMenu;
    public GameObject InstructMenu;
    public GameObject CreatureMenu;
    public GameObject ResourcePage;
    public GameObject RefractPage;
    public GameObject ReflectPage;
    public GameObject BlockPage;
    public GameObject RedirectPage;

    //Vars for buttons/toggles/sliders
    public Button Play;
    public Button Exit;

    public Toggle fullscreenToggle;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

 
    // Use this for initialization
    void Start () {
        Screen.fullScreen = false;
        StartMenu.SetActive(true);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        populateList();
    }
	void Update(){
		setMaster (masterSlider.value);
		setMusic (musicSlider.value);
		setSFX (sfxSlider.value);
	}
	public void startGame()
    {
        SceneManager.LoadScene("main");
    }

    public void mainMenu() {
        StartMenu.SetActive(true);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreatureMenu.SetActive(false);
        ResourcePage.SetActive(false);
        RefractPage.SetActive(false);
        ReflectPage.SetActive(false);
        BlockPage.SetActive(false);
        RedirectPage.SetActive(false);
    }

    public void OptionsMenu()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(true);
        InstructMenu.SetActive(false);
        CreatureMenu.SetActive(false);
        ResourcePage.SetActive(false);
        RefractPage.SetActive(false);
        ReflectPage.SetActive(false);
        BlockPage.SetActive(false);
        RedirectPage.SetActive(false);
    }

    public void InstructionsMenu()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(true);
        CreatureMenu.SetActive(false);
        ResourcePage.SetActive(false);
        RefractPage.SetActive(false);
        ReflectPage.SetActive(false);
        BlockPage.SetActive(false);
        RedirectPage.SetActive(false);
    }

    public void CreautureMenu()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreatureMenu.SetActive(true);
        ResourcePage.SetActive(false);
        RefractPage.SetActive(false);
        ReflectPage.SetActive(false);
        BlockPage.SetActive(false);
        RedirectPage.SetActive(false);
    }

    public void Resource()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreatureMenu.SetActive(false);
        InstructMenu.SetActive(false);
        ResourcePage.SetActive(true);
        RefractPage.SetActive(false);
        ReflectPage.SetActive(false);
        BlockPage.SetActive(false);
        RedirectPage.SetActive(false);
    }

    public void Refract()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreatureMenu.SetActive(false);
        InstructMenu.SetActive(false);
        ResourcePage.SetActive(false);
        RefractPage.SetActive(true);
        ReflectPage.SetActive(false);
        BlockPage.SetActive(false);
        RedirectPage.SetActive(false);
    }

    public void Reflect()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreatureMenu.SetActive(false);
        InstructMenu.SetActive(false);
        ResourcePage.SetActive(false);
        RefractPage.SetActive(false);
        ReflectPage.SetActive(true);
        BlockPage.SetActive(false);
        RedirectPage.SetActive(false);
    }

    public void Block()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreatureMenu.SetActive(false);
        InstructMenu.SetActive(false);
        ResourcePage.SetActive(false);
        RefractPage.SetActive(false);
        ReflectPage.SetActive(false);
        BlockPage.SetActive(true);
        RedirectPage.SetActive(false);
    }

    public void Redirect()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreatureMenu.SetActive(false);
        InstructMenu.SetActive(false);
        ResourcePage.SetActive(false);
        RefractPage.SetActive(false);
        ReflectPage.SetActive(false);
        BlockPage.SetActive(false);
        RedirectPage.SetActive(true);
    }

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
            case 0: Screen.SetResolution(800, 600, true);
                    break;
            case 1: Screen.SetResolution(1024, 768, true);
                    break;
            case 2: Screen.SetResolution(1280, 720, true);
                break;
            case 3: Screen.SetResolution(1920, 1080, true);
                break;
        }
    }
    public void populateList()
    {
        resDrop.AddOptions(resos);
    }
    public void exitGame()
    {
        Application.Quit();
    }
}
