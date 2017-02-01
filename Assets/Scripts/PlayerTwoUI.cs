using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTwoUI : MonoBehaviour {
    public Text playerState;

    // Use this for initialization
    void Start() {
        playerState = GameObject.Find("playerTwoState").GetComponent<Text>();
        playerState.text = "Placing";
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("o")) {
            playerState.text = "swapping";
        }
        if (Input.GetKeyDown("p")) {
            playerState.text = "deleting";
        }
        if (Input.GetKeyDown(";")) {
            playerState.text = "placing";
        }
    }
}