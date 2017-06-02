using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
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
	Image backImage;
	public Sprite davidImage;
	public Sprite ocean;
    public GameObject StartMenu;
    public GameObject OptionMenu;
    public GameObject InstructMenu;
    public GameObject CreatureMenu;
    public GameObject ResourcePage;
    public GameObject RefractPage;
    public GameObject ReflectPage;
    public GameObject BlockPage;
    public GameObject RedirectPage;
    public GameObject ControlsPage;
    public GameObject CreditMenu;
    public GameObject LoadingLogo;
    //Vars for buttons/toggles/sliders
    public Button Play;
    public Button Exit;
    public Toggle fullscreenToggle;
    //Vars for initially selected items
    public Button tutorialButton;
    public Button creatureMenuButton;
    public Dropdown resDropdown;
    public Button resourceInfoButton;

    public Button resourceBack;
    public Button reflectBack;
    public Button refractBack;
    public Button blockBack;
    public Button redirectBack;
    public Button controlBack;
    public Button creditsBack;


    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
	public Slider UISlider;
 
    public Audios[] setUISounds;
    public static Audios[] UISounds;
    public Audios[] setMusicSounds;
    public static Audios[] musicSounds;
    // Use this for initialization
    void Start () {
        UISounds = setUISounds;
        musicSounds = setMusicSounds;
        //Screen.fullScreen = false;
		backImage = GetComponent<Image> ();
		backImage.sprite = davidImage;
        StartMenu.SetActive(true);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreditMenu.SetActive(false);
        populateList();
        if (tutorialToInstructionFlag.instance.flag) mainMenu();
        SoundManager.PlayMusic(musicSounds[0].audioclip, .05f, true, true, 5f, 1.5f);
    }
    void Update(){
		setMaster (masterSlider.value);
		setMusic (musicSlider.value);
		setSFX (sfxSlider.value);
		setUISFX (UISlider.value);
        if(EventSystem.current.IsPointerOverGameObject()){
            SoundManager.PlayUISound(UISounds[0].audioclip, .3f);
        }
	}
	public void startGame()
    {
        SoundManager.PlayUISound(UISounds[0].audioclip, .3f);
        SceneManager.LoadScene("main");
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreditMenu.SetActive(false);
        LoadingLogo.SetActive(true);
    }

	public void startTut(){
        SoundManager.PlayUISound(UISounds[0].audioclip, .3f);
		SceneManager.LoadScene("TutorialBoard");
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreditMenu.SetActive(false);
        LoadingLogo.SetActive(true);
    }

    public void mainMenu() {
        StartMenu.SetActive(true);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreatureMenu.SetActive(false);
        ControlsPage.SetActive(false);
        ResourcePage.SetActive(false);
        RefractPage.SetActive(false);
        ReflectPage.SetActive(false);
        BlockPage.SetActive(false);
        RedirectPage.SetActive(false);
        CreditMenu.SetActive(false);
        backImage.sprite = davidImage;
        SoundManager.PlayUISound(UISounds[0].audioclip, .3f);
        tutorialButton.Select();
        
    }

    public void OptionsMenu()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(true);
        InstructMenu.SetActive(false);
        CreatureMenu.SetActive(false);
        ControlsPage.SetActive(false);
        ResourcePage.SetActive(false);
        RefractPage.SetActive(false);
        ReflectPage.SetActive(false);
        BlockPage.SetActive(false);
        RedirectPage.SetActive(false);
        CreditMenu.SetActive(false);
        backImage.sprite = ocean;
        SoundManager.PlayUISound(UISounds[0].audioclip, .3f);
        fullscreenToggle.Select();
    }

    public void InstructionsMenu()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(true);
        CreatureMenu.SetActive(false);
        ControlsPage.SetActive(false);
        ResourcePage.SetActive(false);
        RefractPage.SetActive(false);
        ReflectPage.SetActive(false);
        BlockPage.SetActive(false);
        RedirectPage.SetActive(false);
        CreditMenu.SetActive(false);
        backImage.sprite = ocean;
        SoundManager.PlayUISound(UISounds[0].audioclip, .3f);
        creatureMenuButton.Select();
    }

    public void CreautureMenu()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreatureMenu.SetActive(true);
        ControlsPage.SetActive(false);
        ResourcePage.SetActive(false);
        RefractPage.SetActive(false);
        ReflectPage.SetActive(false);
        BlockPage.SetActive(false);
        RedirectPage.SetActive(false);
        CreditMenu.SetActive(false);
        backImage.sprite = ocean;
        SoundManager.PlayUISound(UISounds[0].audioclip, .3f);
        resourceInfoButton.Select();
    }

    public void ControlPage()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreatureMenu.SetActive(false);
        ControlsPage.SetActive(true);
        ResourcePage.SetActive(false);
        RefractPage.SetActive(false);
        ReflectPage.SetActive(false);
        BlockPage.SetActive(false);
        RedirectPage.SetActive(false);
        CreditMenu.SetActive(false);
        backImage.sprite = ocean;
        SoundManager.PlayUISound(UISounds[0].audioclip, .3f);
        controlBack.Select();
    }

    public void Resource()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreatureMenu.SetActive(false);
        InstructMenu.SetActive(false);
        ControlsPage.SetActive(false);
        ResourcePage.SetActive(true);
        RefractPage.SetActive(false);
        ReflectPage.SetActive(false);
        BlockPage.SetActive(false);
        RedirectPage.SetActive(false);
        CreditMenu.SetActive(false);
        backImage.sprite = ocean;
        SoundManager.PlayUISound(UISounds[0].audioclip, .3f);
        resourceBack.Select();
    }

    public void Refract()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreatureMenu.SetActive(false);
        InstructMenu.SetActive(false);
        ControlsPage.SetActive(false);
        ResourcePage.SetActive(false);
        RefractPage.SetActive(true);
        ReflectPage.SetActive(false);
        BlockPage.SetActive(false);
        RedirectPage.SetActive(false);
        CreditMenu.SetActive(false);
        backImage.sprite = ocean;
        SoundManager.PlayUISound(UISounds[0].audioclip, .3f);
        refractBack.Select();
    }

    public void Reflect()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreatureMenu.SetActive(false);
        InstructMenu.SetActive(false);
        ControlsPage.SetActive(false);
        ResourcePage.SetActive(false);
        RefractPage.SetActive(false);
        ReflectPage.SetActive(true);
        BlockPage.SetActive(false);
        RedirectPage.SetActive(false);
        CreditMenu.SetActive(false);
        backImage.sprite = ocean;
        SoundManager.PlayUISound(UISounds[0].audioclip, .3f);
        reflectBack.Select();
    }

    public void Block()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreatureMenu.SetActive(false);
        InstructMenu.SetActive(false);
        ControlsPage.SetActive(false);
        ResourcePage.SetActive(false);
        RefractPage.SetActive(false);
        ReflectPage.SetActive(false);
        BlockPage.SetActive(true);
        RedirectPage.SetActive(false);
        CreditMenu.SetActive(false);
        backImage.sprite = ocean;
        SoundManager.PlayUISound(UISounds[0].audioclip, .3f);
        blockBack.Select();
    }

    public void Redirect()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreatureMenu.SetActive(false);
        InstructMenu.SetActive(false);
        ControlsPage.SetActive(false);
        ResourcePage.SetActive(false);
        RefractPage.SetActive(false);
        ReflectPage.SetActive(false);
        BlockPage.SetActive(false);
        RedirectPage.SetActive(true);
        CreditMenu.SetActive(false);
        backImage.sprite = ocean;
        SoundManager.PlayUISound(UISounds[0].audioclip, .3f);
        redirectBack.Select();
    }

    public void CreditsMenu()
    {
        StartMenu.SetActive(false);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(false);
        CreatureMenu.SetActive(false);
        ControlsPage.SetActive(false);
        ResourcePage.SetActive(false);
        RefractPage.SetActive(false);
        ReflectPage.SetActive(false);
        BlockPage.SetActive(false);
        RedirectPage.SetActive(false);
        CreditMenu.SetActive(true);
        backImage.sprite = ocean;
        SoundManager.PlayUISound(UISounds[0].audioclip, .3f);
        creditsBack.Select();
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
	public void setUISFX(float vol)
	{
		SoundManager.globalUISoundsVolume = vol;
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
        //resDrop.AddOptions(resos);
    }
    public void exitGame()
    {
        Application.Quit();
    }

    void OnDisable()
    {
        tutorialToInstructionFlag.instance.flag = false;
    }
}
