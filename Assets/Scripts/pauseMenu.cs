using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{

    public GameObject pauseMenuCanvas;
    public Button Pause;
    public Button Resume;
    /* --- Unimplemented Buttons ---
    public Button Instructions;
    public Button Options;
       --- Unimplemented Buttons ---*/
    public Button End;
    public bool isPaused = false;

    // Use this for initialization
    void Start()
    {
        pauseMenuCanvas = GameObject.Find("Pause Menu");
        pauseMenuCanvas.SetActive(false);
        Pause = Pause.GetComponent<Button>();
        //Resume = Resume.GetComponent<Button>();
        // Instructions = Instructions.GetComponent<Button>();
        // Options = Options.GetComponent<Button>();
        //End = End.GetComponent<Button>();
    }

    public void pauseGame()
    {
        // Stops Update functions, essentially pausing the game
        Time.timeScale = 0F;
        isPaused = true;
        pauseMenuCanvas.SetActive(true);
    }

    public void resumeGame()
    {
        // Sets the game time to realtime
        Time.timeScale = 1F;
        isPaused = false;
        pauseMenuCanvas.SetActive(false);
    }

    public void endGame()
    {
        SceneManager.LoadScene("TitleScreen");
    }

}