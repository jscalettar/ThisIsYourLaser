using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOver : MonoBehaviour {

    public void startGame()
    {
        SceneManager.LoadScene("main");
    }

    public void backMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
